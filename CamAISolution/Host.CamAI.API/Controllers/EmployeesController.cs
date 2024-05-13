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
public class EmployeesController(
    IAccountService accountService,
    IServiceProvider serviceProvider,
    IEmployeeService employeeService,
    IBaseMapping mapper,
    IBulkTaskService bulkTaskService
) : ControllerBase
{
    /// <summary>
    /// Search employees
    /// </summary>
    /// <remarks>
    /// <para>
    /// Admin can get all employees<br/>
    /// Brand manager can get all employees working for their brand (the <c>BrandId</c> field is ignored)<br/>
    /// Shop manager can get all employees working for their shop (the <c>BrandId</c> and <c>ShopId</c> fields are ignored)
    /// </para>
    /// <para><c>Search</c> can be Email, Name or Phone</para>
    /// </remarks>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpGet]
    [AccessTokenGuard(Role.ShopManager, Role.BrandManager, Role.Admin, Role.ShopHeadSupervisor, Role.ShopSupervisor)]
    public async Task<PaginationResult<EmployeeDto>> Get([FromQuery] SearchEmployeeRequest req)
    {
        return mapper.Map<Employee, EmployeeDto>(await employeeService.GetEmployees(req));
    }

    [HttpGet("{id}")]
    [AccessTokenGuard(Role.ShopManager, Role.BrandManager, Role.Admin)]
    public async Task<EmployeeDto> Get(Guid id)
    {
        return mapper.Map<Employee, EmployeeDto>(await employeeService.GetEmployeeById(id));
    }

    [HttpPost]
    [AccessTokenGuard(Role.ShopManager)]
    public async Task<EmployeeDto> Create(CreateEmployeeDto dto)
    {
        return mapper.Map<Employee, EmployeeDto>(await employeeService.CreateEmployee(dto));
    }

    [HttpPut("{id}")]
    [AccessTokenGuard(Role.ShopManager)]
    public async Task<EmployeeDto> Update(Guid id, UpdateEmployeeDto dto)
    {
        return mapper.Map<Employee, EmployeeDto>(await employeeService.UpdateEmployee(id, dto));
    }

    /// <remarks>If the employee does not have any incident, they will be removed,
    /// otherwise their status will be updated to <c>Inactive</c></remarks>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [AccessTokenGuard(Role.ShopManager)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await employeeService.DeleteEmployee(id);
        return Accepted();
    }

    /// <summary>
    /// Only shop manager con upsert employee for their current managed shop
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    /// <exception cref="BadRequestException"></exception>
    [HttpPost("upsert")]
    [RequestSizeLimit(10_000_000)]
    [AccessTokenGuard(Role.ShopManager)]
    public async Task<ActionResult<BulkResponse>> UpsertEmployees(IFormFile file)
    {
        if (!file.ContentType.Equals("text/csv", StringComparison.CurrentCultureIgnoreCase))
            throw new BadRequestException("Accept.csv format only");
        var shopManagerId = accountService.GetCurrentAccount().Id;
        var bulkTaskId = Guid.NewGuid().ToString("N");
        var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        var scope = serviceProvider.CreateScope();
        var bulkTask = Task.Run(async () =>
        {
            var scopeEmployeeService = scope.ServiceProvider.GetRequiredService<IEmployeeService>();
            var jwtService = scope.ServiceProvider.GetRequiredService<IJwtService>();
            stream.Seek(0, SeekOrigin.Begin);
            await jwtService.SetCurrentUserToSystemHandler();
            var result = await scopeEmployeeService
                .UpsertEmployees(shopManagerId, stream, bulkTaskId)
                .ContinueWith(t =>
                {
                    bulkTaskService.RemoveTaskById(shopManagerId, bulkTaskId);
                    scope.Dispose();
                    return t.Result;
                });
            return result;
        });
        bulkTaskService.AddUpsertTask(shopManagerId, bulkTask, bulkTaskId);
        var res = new BulkResponse() { TaskId = bulkTaskId, Message = "Task is accepted" };
        return Ok(res);
    }

    /// <summary>
    /// Long polling to get result of upsert task (Shop manager only)
    /// </summary>
    /// <param name="taskId">String value returned by /api/employees/upsert</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("upsert/task/{taskId}/result")]
    [AccessTokenGuard(Role.ShopManager)]
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
                    Percent = progress.CurrentFinishedRecord * 100f / progress.Total,
                    Detailed = new { progress.CurrentFinishedRecord, progress.Total }
                }
            )
        );
    }

    /// <summary>
    /// Shop manager get all upsert tasks are in process
    /// </summary>
    /// <returns></returns>
    [HttpGet("upsert/task/")]
    [AccessTokenGuard(Role.ShopManager)]
    public ActionResult<List<string>> GetUpsertTaskIds()
    {
        bulkTaskService.GetTaskByActorId(accountService.GetCurrentAccount().Id, out var taskIds);
        return taskIds;
    }
}
