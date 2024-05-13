using Core.Application.Exceptions;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Services;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShopsController(
    IAppLogging<ShopsController> logger,
    IAccountService accountService,
    ISupervisorAssignmentService supervisorAssignmentService,
    IServiceProvider serviceProvider,
    IShopService shopService,
    IBaseMapping baseMapping,
    IBulkTaskService bulkTaskService
) : ControllerBase
{
    /// <summary>
    /// Search Shop base on current account's roles.
    /// </summary>
    /// <param name="search"></param>
    /// <remarks>
    /// <para>
    ///     Admin can search every shops.
    /// </para>
    /// <para>
    ///     Brand manager can search every shops in his/her brand.
    /// </para>
    /// <para>
    ///     Shop manager can only see his/her shop
    /// </para>
    /// </remarks>
    /// <returns></returns>
    [HttpGet]
    [AccessTokenGuard(Role.Admin, Role.BrandManager, Role.ShopManager)]
    public async Task<ActionResult<PaginationResult<ShopDto>>> GetCurrentShop([FromQuery] ShopSearchRequest search)
    {
        var shops = await shopService.GetShops(search);
        return Ok(baseMapping.Map<Shop, ShopDto>(shops));
    }

    [HttpGet("{id}")]
    [AccessTokenGuard(Role.Admin, Role.BrandManager, Role.ShopManager)]
    public async Task<ActionResult<ShopDto>> GetShopById(Guid id)
    {
        return Ok(baseMapping.Map<Shop, ShopDto>(await shopService.GetShopById(id)));
    }

    [HttpPost]
    [AccessTokenGuard(Role.BrandManager)]
    public async Task<ActionResult<ShopDto>> CreateShop(CreateOrUpdateShopDto shopDto)
    {
        return Ok(baseMapping.Map<Shop, ShopDto>(await shopService.CreateShop(shopDto)));
    }

    [HttpPut("{id:guid}")]
    [AccessTokenGuard(Role.BrandManager)]
    public async Task<ActionResult<ShopDto>> UpdateShop(Guid id, [FromBody] CreateOrUpdateShopDto shopDto)
    {
        var updatedShop = await shopService.UpdateShop(id, shopDto);
        return Ok(baseMapping.Map<Shop, ShopDto>(updatedShop));
    }

    [HttpPatch("{id:guid}/status/{shopStatus}")]
    [AccessTokenGuard(Role.BrandManager)]
    public async Task<ActionResult<ShopDto>> UpdateShopStatus(Guid id, ShopStatus shopStatus)
    {
        var updatedShop = await shopService.UpdateShopStatus(id, shopStatus);
        if (updatedShop.ShopStatus == ShopStatus.Inactive)
            return base.Ok();
        return Ok(baseMapping.Map<Shop, ShopDto>(updatedShop));
    }

    [HttpDelete("{id:guid}")]
    [AccessTokenGuard(Role.Admin, Role.BrandManager)]
    public async Task<IActionResult> DeleteShop(Guid id)
    {
        await shopService.DeleteShop(id);
        return Accepted();
    }

    /// <summary>
    /// Get all shops that currently have edge box installing or not.
    /// </summary>
    /// <remarks>For role Admin</remarks>
    /// <param name="q"></param>
    /// <returns></returns>
    [HttpGet("installing")]
    [AccessTokenGuard(Role.Admin)]
    public async Task<ActionResult<PaginationResult<ShopDto>>> GetShopsInstallingEdgeBox(bool q)
    {
        var shops = await shopService.GetShopsInstallingEdgeBox(q);
        return Ok(baseMapping.Map<Shop, ShopDto>(shops));
    }

    /// <summary>
    /// only brand Manager upsert shop and shop manager
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost("upsert")]
    [RequestSizeLimit(10_000_000)]
    [AccessTokenGuard(Role.BrandManager)]
    public async Task<ActionResult<BulkResponse>> UpsertShopAndManager(IFormFile file)
    {
        if (!file.ContentType.Equals("text/csv", StringComparison.CurrentCultureIgnoreCase))
            throw new BadRequestException("Accept.csv format only");
        var brandManagerId = accountService.GetCurrentAccount().Id;
        var bulkTaskId = Guid.NewGuid().ToString("N");
        var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        var scope = serviceProvider.CreateScope();
        var bulkTask = Task.Run(async () =>
        {
            var scopeShopService = scope.ServiceProvider.GetRequiredService<IShopService>();
            var jwtService = scope.ServiceProvider.GetRequiredService<IJwtService>();
            stream.Seek(0, SeekOrigin.Begin);
            await jwtService.SetCurrentUserToSystemHandler();
            var result = await scopeShopService
                .UpsertShops(brandManagerId, stream, bulkTaskId)
                .ContinueWith(t =>
                {
                    bulkTaskService.RemoveTaskById(brandManagerId, bulkTaskId);
                    scope.Dispose();
                    return t.Result;
                });
            return result;
        });
        var res = new BulkResponse { TaskId = bulkTaskId, Message = "Task is accepted" };
        bulkTaskService.AddUpsertTask(brandManagerId, bulkTask, bulkTaskId);
        return Ok(res);
    }

    /// <summary>
    /// Long polling to get result of upsert task
    /// </summary>
    /// <param name="taskId">String value returned by /api/shops/upsert</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("upsert/task/{taskId}/result")]
    [AccessTokenGuard(Role.BrandManager)]
    public async Task<ActionResult<BulkUpsertTaskResultResponse>> GetUpsertTaskResult(
        string taskId,
        CancellationToken cancellationToken
    )
    {
        var result = await bulkTaskService.GetBulkUpsertTaskResultResponse(
            accountService.GetCurrentAccount().Id,
            taskId,
            cancellationToken,
            TimeSpan.FromMinutes(5)
        );
        if (result != null)
            return result;
        return NoContent();
    }

    /// <summary>
    /// Get task progression
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns></returns>
    [HttpGet("upsert/task/{taskId}/progress")]
    public Task<IActionResult> GetTaskProgress(string taskId)
    {
        var progress = bulkTaskService.GetTaskProgress(taskId);
        return Task.FromResult<IActionResult>(
            Ok(
                new
                {
                    Percents = progress.CurrentFinishedRecord * 100f / progress.Total,
                    Detailed = new { progress.CurrentFinishedRecord, progress.Total }
                }
            )
        );
    }

    /// <summary>
    /// Get all tasks are in process
    /// </summary>
    /// <returns></returns>
    [HttpGet("upsert/task")]
    [AccessTokenGuard(Role.BrandManager)]
    public ActionResult<List<string>> GetUpsertTaskIds()
    {
        bulkTaskService.GetTaskByActorId(accountService.GetCurrentAccount().Id, out var taskIds);
        return taskIds;
    }

    [HttpPost("supervisor")]
    [AccessTokenGuard(Role.ShopHeadSupervisor, Role.ShopManager)]
    public async Task<SupervisorAssignmentDto> AssignShopSupervisor(AssignShopSupervisorDto dto)
    {
        var assignment = await shopService.AssignSupervisorRoles(dto.AccountId, dto.Role);
        return ToSupervisorAssignmentDto(assignment);
    }

    private SupervisorAssignmentDto ToSupervisorAssignmentDto(SupervisorAssignment supervisorAssignment)
    {
        var dto = baseMapping.Map<SupervisorAssignment, SupervisorAssignmentDto>(supervisorAssignment);
        var inCharge =
            supervisorAssignment.Supervisor
            ?? supervisorAssignment.HeadSupervisor
            ?? accountService.GetCurrentAccount();
        dto.InCharge = baseMapping.Map<Account, AccountDto>(inCharge);
        dto.InChargeId = inCharge.Id;
        dto.InChargeRole = inCharge.Role;
        return dto;
    }

    /// <summary>
    /// Assign supervisor from employee Id
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("employee/supervisor")]
    [AccessTokenGuard(Role.ShopHeadSupervisor, Role.ShopManager)]
    public async Task<SupervisorAssignmentDto> AssignShopSupervisorFromEmployee(AssignShopSupervisorFromEmployeeDto dto)
    {
        var assignment = await shopService.AssignSupervisorRolesFromEmployee(dto.EmployeeId, dto.Role);
        return ToSupervisorAssignmentDto(assignment);
    }

    /// <summary>
    /// Remove current head supervisor and set in charge to shop manager
    /// </summary>
    [HttpDelete("headsupervisor")]
    [AccessTokenGuard(Role.ShopManager)]
    public async Task RemoveHeadSupervisor()
    {
        await supervisorAssignmentService.RemoveHeadSupervisor();
    }

    /// <summary>
    /// Remove current supervisor and set in charge to head supervisor
    /// </summary>
    [HttpDelete("supervisor")]
    [AccessTokenGuard(Role.ShopManager)]
    public async Task RemoveSupervisor()
    {
        await supervisorAssignmentService.RemoveSupervisor();
    }

    [HttpGet("isIncharge")]
    [AccessTokenGuard(Role.ShopManager, Role.ShopSupervisor, Role.ShopHeadSupervisor)]
    public async Task<bool> IsInCharge()
    {
        return await shopService.IsInCharge();
    }
}
