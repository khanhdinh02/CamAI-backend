using System.Collections.Concurrent;
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
public class EmployeesController(IAccountService accountService, IServiceProvider serviceProvider, IEmployeeService employeeService, IBaseMapping mapper) : ControllerBase
{
    private static readonly ConcurrentDictionary<string, Task<BulkUpsertTaskResultResponse>> Tasks = new();

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
    [AccessTokenGuard(Role.ShopManager, Role.BrandManager, Role.Admin)]
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

    [HttpPost("upsert")]
    [RequestSizeLimit(10_000_000)]
    [AccessTokenGuard(Role.ShopManager)]
    public async Task<IActionResult> UpsertEmployees(IFormFile file)
    {
        var shopManagerId = accountService.GetCurrentAccount().Id;
        var bulkTaskId = Guid.NewGuid().ToString("N");
        var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        var bulkTask = Task.Run(async () =>
        {
            using var scope = serviceProvider.CreateScope();
            try
            {
                var scopeEmployeeService = scope.ServiceProvider.GetRequiredService<IEmployeeService>();
                var jwtService = scope.ServiceProvider.GetRequiredService<IJwtService>();
                stream.Seek(0, SeekOrigin.Begin);
                await jwtService.SetCurrentUserToSystemHandler();
                var result = await scopeEmployeeService.UpsertEmployees(shopManagerId, stream);
                return result;
            }
            catch (Exception ex)
            {
                scope.ServiceProvider.GetRequiredService<IAppLogging<EmployeesController>>().Error(ex.Message, ex);
            }
            finally
            {
                await stream.DisposeAsync();
                Tasks.TryRemove(bulkTaskId, out _);
            }

            return new BulkUpsertTaskResultResponse(0 ,0, 0);
        });
        Tasks.TryAdd(bulkTaskId, bulkTask);

        var res = new BulkResponse()
        {
            TaskId = bulkTaskId,
            Message = "Task is accepted"
        };
        return Ok(res);
    }

    /// <summary>
    /// Long polling to get result of upsert task
    /// </summary>
    /// <param name="taskId">String value returned by /api/employees/upsert</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("upsert/task/{taskId}/result")]
    public async Task<IActionResult> GetUpsertTaskResult(string taskId, CancellationToken cancellationToken)
    {
        if (!Tasks.TryGetValue(taskId, out var bulkTask))
            return NoContent();
        using var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        tokenSource.CancelAfter(TimeSpan.FromMinutes(2));

        var timeoutTask = Task.Delay(-1, tokenSource.Token);
        var completedTask = await Task.WhenAny(timeoutTask, bulkTask);
        if (completedTask == bulkTask)
            return Ok(await bulkTask);
        return NoContent();
    }
}
