using Core.Application.Events;
using Core.Application.Exceptions;
using Core.Application.Specifications.Repositories;
using Core.Domain;
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
    IAppLogging<EmployeeService> logger,
    IBaseMapping mapper,
    BulkUpsertProgressSubject bulkUpsertProgressSubject
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
        else if (user.Role is Role.ShopSupervisor or Role.ShopHeadSupervisor)
        {
            var employee = (await unitOfWork.Employees.GetAsync(x => x.AccountId == user.Id)).Values.FirstOrDefault();
            if (employee == null)
                throw new BadRequestException($"{user.Role} is not linked to any employee");

            req.ShopId = employee.ShopId;
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
            var account = (await unitOfWork.Accounts.GetAsync(x => x.Email == dto.Email)).Values.FirstOrDefault();
            if (
                oldEmp.EmployeeStatus != EmployeeStatus.Inactive
                || (account != null && account.AccountStatus != AccountStatus.Inactive)
            )
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

        var account = (await unitOfWork.Accounts.GetAsync(x => x.Email == dto.Email)).Values.FirstOrDefault();
        if (
            (
                (await unitOfWork.Employees.GetAsync(e => e.Email == dto.Email)).Values.FirstOrDefault() is { } oldEmp
                && oldEmp.Id != id
            )
            || (account != null && account.AccountStatus != AccountStatus.Inactive && employee.AccountId != account.Id)
        )
            throw new BadRequestException($"Email {dto.Email} is already taken");

        if (dto.WardId != null && !await unitOfWork.Wards.IsExisted(dto.WardId))
            throw new NotFoundException(typeof(Ward), dto.WardId);

        unitOfWork.Employees.Update(mapper.Map(dto, employee));
        if (employee.AccountId != null)
        {
            var updateAccount = await accountService.GetAccountById(employee.AccountId.Value);
            updateAccount.Email = dto.Email;
            updateAccount.Name = dto.Name;
            updateAccount.Phone = dto.Phone;
            updateAccount.AddressLine = dto.AddressLine;
            updateAccount.WardId = dto.WardId;
            unitOfWork.Accounts.Update(updateAccount);
        }
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

        if (employee.AccountId != null)
            await accountService.DeleteAccount(employee.AccountId.Value);
        await unitOfWork.CompleteAsync();
    }

    public async Task<BulkUpsertTaskResultResponse> UpsertEmployees(Guid actorId, MemoryStream stream, string taskId)
    {
        var employeeInserted = new HashSet<Guid>();
        var employeeUpdated = new HashSet<Guid>();
        var failedValidatedRecords = new Dictionary<int, object?>();
        var rowCount = 1;
        var shop =
            (await unitOfWork.Shops.GetAsync(s => s.ShopManagerId == actorId)).Values.FirstOrDefault()
            ?? throw new NotFoundException("Cannot found shop");
        await unitOfWork.BeginTransaction();
        try
        {
            foreach (
                var record in readFileService.ReadFromCsv<EmployeeFromImportFile>(
                    stream,
                    true,
                    $"total-records-{taskId}"
                )
            )
            {
                bulkUpsertProgressSubject.Notify(new(rowCount++, taskId));
                if (!record.IsValid())
                {
                    failedValidatedRecords.Add(rowCount, record.EmployeeFromImportFileValidation());
                    continue;
                }

                var employee = (
                    await unitOfWork.Employees.GetAsync(
                        expression: e =>
                            (!string.IsNullOrEmpty(record.ExternalId) && record.ExternalId == e.ExternalId)
                            || (!string.IsNullOrEmpty(record.Email) && record.Email == e.Email),
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
                        AddressLine = record.AddressLine,
                        EmployeeStatus = EmployeeStatus.Active
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
                    if (employee.AccountId != null)
                    {
                        var account = await unitOfWork.Accounts.GetByIdAsync(employee.AccountId);
                        if (account == null)
                        {
                            failedValidatedRecords.Add(
                                rowCount,
                                new Dictionary<string, object?>
                                {
                                    { "Account not found", "Account related to employee not found" }
                                }
                            );
                            continue;
                        }

                        if (record.Email == null)
                        {
                            failedValidatedRecords.Add(
                                rowCount,
                                new Dictionary<string, object?>
                                {
                                    {
                                        "Email is empty",
                                        "Employee is linked with an account so email must not be empty"
                                    }
                                }
                            );
                            continue;
                        }

                        account.Email = record.Email;
                        account.Phone = record.Phone;
                        account.AddressLine = record.AddressLine;
                        account.Name = record.Name;
                    }
                    unitOfWork.Employees.Update(employee);
                    employeeUpdated.Add(employee.Id);
                }
            }

            await unitOfWork.CompleteAsync();
            await unitOfWork.CommitTransaction();
            await notificationService.CreateNotification(
                new(taskId)
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
                BulkUpsertStatus.Success,
                employeeInserted.Count,
                employeeUpdated.Count,
                failedValidatedRecords.Count,
                "",
                new { EmployeeInserted = employeeInserted },
                new { EmployeeUpdated = employeeUpdated },
                new { Errors = failedValidatedRecords.Select(e => new { Row = e.Key, Reasons = e.Value }) }
            );
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
            await unitOfWork.RollBack();
        }
        finally
        {
            stream.Close();
        }
        await notificationService.CreateNotification(
            new(taskId)
            {
                Priority = NotificationPriority.Urgent,
                Content = "Upsert failed",
                Type = NotificationType.UpsertEmployee,
                SentToId = [actorId],
                Title = "Upsert Failed",
            }
        );
        return new BulkUpsertTaskResultResponse(BulkUpsertStatus.Fail, 0, 0, 0, "Employee upsert failed");
    }

    public async Task<Employee?> GetEmployeeAccount(Guid accountId)
    {
        return (await unitOfWork.Employees.GetAsync(x => x.AccountId == accountId)).Values.FirstOrDefault();
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
