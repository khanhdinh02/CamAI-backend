using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController(IEmployeeService employeeService, IBaseMapping mapper) : ControllerBase
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
    [AccessTokenGuard(RoleEnum.ShopManager, RoleEnum.BrandManager, RoleEnum.Admin)]
    public async Task<PaginationResult<EmployeeDto>> Get([FromQuery] SearchEmployeeRequest req)
    {
        return mapper.Map<Employee, EmployeeDto>(await employeeService.GetEmployees(req));
    }

    [HttpGet("{id}")]
    [AccessTokenGuard(RoleEnum.ShopManager, RoleEnum.BrandManager, RoleEnum.Admin)]
    public async Task<EmployeeDto> Get(Guid id)
    {
        return mapper.Map<Employee, EmployeeDto>(await employeeService.GetEmployeeById(id));
    }

    [HttpPost]
    [AccessTokenGuard(RoleEnum.ShopManager)]
    public async Task<EmployeeDto> Create(CreateEmployeeDto dto)
    {
        return mapper.Map<Employee, EmployeeDto>(await employeeService.CreateEmployee(dto));
    }

    [HttpPut("{id}")]
    [AccessTokenGuard(RoleEnum.ShopManager)]
    public async Task<EmployeeDto> Update(Guid id, UpdateEmployeeDto dto)
    {
        return mapper.Map<Employee, EmployeeDto>(await employeeService.UpdateEmployee(id, dto));
    }

    [HttpDelete("{id}")]
    [AccessTokenGuard(RoleEnum.ShopManager)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await employeeService.DeleteEmployee(id);
        return Accepted();
    }

    [HttpGet("{id}/shifts")]
    [AccessTokenGuard(RoleEnum.ShopManager, RoleEnum.BrandManager, RoleEnum.Admin)]
    public async Task<IEnumerable<EmployeeShiftDto>> GetShifts(Guid id)
    {
        return (await employeeService.GetShifts(id)).Select(mapper.Map<EmployeeShift, EmployeeShiftDto>);
    }
}
