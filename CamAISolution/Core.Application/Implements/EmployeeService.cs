using Core.Application.Exceptions;
using Core.Application.Specifications.Repositories;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Repositories;

namespace Core.Application.Implements;

public class EmployeeService(
    INotificationService notificationService,
    IUnitOfWork unitOfWork,
    IAccountService accountService,
    IReadFileService readFileService,
    IBaseMapping mapper
) : IEmployeeService
{
    public async Task<PaginationResult<Employee>> GetEmployees(SearchEmployeeRequest req)
    {
        var user = accountService.GetCurrentAccount();
        if (user.Role == Role.BrandManager)
            req.BrandId = user.BrandId;
        else if (user.Role == Role.ShopManager)
        {
            if (user.ManagingShop == null)
                throw new ForbiddenException("Shop manager must be assigned to a shop");
            req.BrandId = null;
            req.ShopId = user.ManagingShop.Id;
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

        if (oldEmp != null && dto.Email != null)
        {
            if (oldEmp.EmployeeStatus != EmployeeStatus.Inactive)
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
        newEmp.EmployeeStatus = EmployeeStatus.Active;

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
        var employee =
            (
                await unitOfWork.Employees.GetAsync(e => e.Id == id, includeProperties: [nameof(Employee.Incidents)])
            ).Values.FirstOrDefault() ?? throw new NotFoundException(typeof(Employee), id);
        if (!HasAuthority(accountService.GetCurrentAccount(), employee))
            throw new ForbiddenException(accountService.GetCurrentAccount(), employee);
        if (employee.Incidents.Count == 0)
            unitOfWork.Employees.Delete(employee);
        else
        {
            employee.EmployeeStatus = EmployeeStatus.Inactive;
            unitOfWork.Employees.Update(employee);
        }
        await unitOfWork.CompleteAsync();
    }

    public async Task<BulkUpsertTaskResultResponse> UpsertEmployees(Guid actorId, MemoryStream stream)
    {
        var employeeInserted = new HashSet<Guid>();
        var employeeUpdated = new HashSet<Guid>();
        var failedValidatedRecords = new Dictionary<int, object?>();
        var rowCount = 1;
        var shop =
            (await unitOfWork.Shops.GetAsync(s => s.ShopManagerId == actorId)).Values.FirstOrDefault()
            ?? throw new NotFoundException("Cannot found shop");
        await unitOfWork.BeginTransaction();
        foreach (var record in readFileService.ReadFromCsv<EmployeeFromImportFile>(stream))
        {
            rowCount++;
            if (!record.IsValid())
            {
                failedValidatedRecords.Add(rowCount, record.EmployeeFromImportFileValidation());
                continue;
            }

            var employee = (
                await unitOfWork.Employees.GetAsync(
                    expression: e =>
                        (record.ExternalId != null && record.ExternalId == e.ExternalId)
                        || (record.Email != null && record.Email == e.Email),
                    disableTracking: false
                )
            ).Values.FirstOrDefault();
            if (employee == null)
            {
                employee = new()
                {
                    Name = record.Name,
                    Gender = record.Gender ?? Gender.Male,
                    ExternalId = record.ExternalId,
                    Email = record.Email == string.Empty ? null : record.Email,
                    ShopId = shop.Id,
                    Birthday = record.Birthday,
                    AddressLine = record.AddressLine
                };
                employee = await unitOfWork.Employees.AddAsync(employee);
                employeeInserted.Add(employee.Id);
            }
            else
            {
                employee.Name = record.Name;
                employee.Gender = record.Gender ?? Gender.Male;
                employee.ExternalId = record.ExternalId;
                employee.Email = record.Email == string.Empty ? null : record.Email;
                employee.ShopId = shop.Id;
                employee.Birthday = record.Birthday;
                employee.AddressLine = record.AddressLine;
                unitOfWork.Employees.Update(employee);
                employeeUpdated.Add(employee.Id);
            }
        }
        await unitOfWork.CompleteAsync();
        await unitOfWork.CommitTransaction();
        await notificationService.CreateNotification(
            new()
            {
                Priority = NotificationPriority.Normal,
                Content =
                    $"Inserted: {employeeInserted.Count}\nUpdated: {employeeUpdated.Count}\nFailed:{failedValidatedRecords.Count}",
                Title = "Upsert employees completed",
                Type = NotificationType.UpsertEmployee,
                SentToId = [actorId],
            }
        );
        return new(
            employeeInserted.Count,
            employeeUpdated.Count,
            failedValidatedRecords.Count,
            new { EmployeeInserted = employeeInserted },
            new { EmployeeUpdated = employeeUpdated },
            new { Errors = failedValidatedRecords.Select(e => new { Row = e.Key, Reasons = e.Value }) }
        );
    }

    private bool HasAuthority(Account user, Employee employee)
    {
        if (user.Role == Role.Admin)
            return true;
        if (user.Role == Role.BrandManager)
            return user.BrandId == employee.Shop?.BrandId;
        if (user is { Role: Role.ShopManager, ManagingShop: not null })
            return user.ManagingShop.Id == employee.ShopId;
        return false;
    }
}
