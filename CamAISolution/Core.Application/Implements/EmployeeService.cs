using Core.Application.Exceptions;
using Core.Application.Specifications.Repositories;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Repositories;
using Core.Domain.Utilities;

namespace Core.Application.Implements;

// TODO [Khanh]: What authority do brand manager and shop manager have over employees?
// TODO: Sync with edge box
public class EmployeeService(IUnitOfWork unitOfWork, IAccountService accountService, IBaseMapping mapper)
    : IEmployeeService
{
    public async Task<PaginationResult<Employee>> GetEmployees(SearchEmployeeRequest req)
    {
        var user = accountService.GetCurrentAccount();
        if (!user.HasRole(RoleEnum.Admin))
        {
            if (user.HasRole(RoleEnum.BrandManager))
                req.BrandId = user.BrandId;
            else if (user.HasRole(RoleEnum.ShopManager))
            {
                if (user.ManagingShop == null)
                    throw new ForbiddenException("Shop manager must be assigned to a shop");
                req.BrandId = null;
                req.ShopId = user.ManagingShop.Id;
            }
        }

        return await unitOfWork.Employees.GetAsync(new EmployeeSearchSpec(req));
    }

    public async Task<Employee> GetEmployeeById(Guid id)
    {
        var user = accountService.GetCurrentAccount();
        var employee =
            (await unitOfWork.Employees.GetAsync(new EmployeeByIdRepoSpec(id))).Values.FirstOrDefault()
            ?? throw new NotFoundException(typeof(Employee), id);
        if (!HasAuthority(user, employee))
            throw new ForbiddenException(user, employee);
        return employee;
    }

    public async Task<Employee> CreateEmployee(CreateEmployeeDto dto)
    {
        Employee newEmp;

        var user = accountService.GetCurrentAccount();
        var oldEmp = (await unitOfWork.Employees.GetAsync(e => e.Email == dto.Email)).Values.FirstOrDefault();

        if (oldEmp != null)
        {
            if (oldEmp.EmployeeStatusId != EmployeeStatusEnum.Inactive)
                throw new BadRequestException($"Email {dto.Email} is already taken");
            newEmp = new Employee { Id = oldEmp.Id, Timestamp = oldEmp.Timestamp };
        }
        else
        {
            newEmp = new Employee();
            await unitOfWork.Employees.AddAsync(newEmp);
        }

        if (dto.WardId != null && !await unitOfWork.Wards.IsExisted(dto.WardId))
            throw new NotFoundException(typeof(Ward), dto.WardId);

        if (user.ManagingShop == null)
            throw new ForbiddenException("Shop manager must be assigned to a shop");

        mapper.Map(dto, newEmp);
        newEmp.ShopId = user.ManagingShop.Id;
        newEmp.EmployeeStatusId = EmployeeStatusEnum.Active;

        unitOfWork.Employees.Update(newEmp);
        await unitOfWork.CompleteAsync();
        return newEmp;
    }

    public async Task<Employee> UpdateEmployee(Guid id, UpdateEmployeeDto dto)
    {
        var employee = await GetEmployeeById(id);

        if (
            (await unitOfWork.Employees.GetAsync(e => e.Email == dto.Email)).Values.FirstOrDefault() is { } oldEmp
            && oldEmp.Id != id
        )
            throw new BadRequestException($"Email {dto.Email} is already taken");

        if (dto.WardId != null && !await unitOfWork.Wards.IsExisted(dto.WardId))
            throw new NotFoundException(typeof(Ward), dto.WardId);

        unitOfWork.Employees.Update(mapper.Map(dto, employee));
        await unitOfWork.CompleteAsync();
        return employee;
    }

    public async Task DeleteEmployee(Guid id)
    {
        var employee = await GetEmployeeById(id);
        employee.EmployeeStatusId = EmployeeStatusEnum.Inactive;
        employee.ShopId = null;
        unitOfWork.Employees.Update(employee);
        await unitOfWork.CompleteAsync();
    }

    private bool HasAuthority(Account user, Employee employee)
    {
        if (user.HasRole(RoleEnum.Admin))
            return true;
        if (user.HasRole(RoleEnum.BrandManager))
            return user.BrandId == employee.Shop?.BrandId;
        if (user.HasRole(RoleEnum.ShopManager) && user.ManagingShop != null)
            return user.ManagingShop.Id == employee.ShopId;
        return false;
    }
}
