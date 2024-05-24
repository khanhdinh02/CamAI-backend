using Core.Application.Events;
using Core.Application.Exceptions;
using Core.Application.Specifications.Repositories;
using Core.Application.Specifications.Shops.Repositories;
using Core.Domain;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Models;
using Core.Domain.Repositories;
using Core.Domain.Services;
using Core.Domain.Utilities;

namespace Core.Application.Implements;

public class ShopService(
    IUnitOfWork unitOfWork,
    IEdgeBoxService edgeBoxService,
    IAppLogging<ShopService> logger,
    IBaseMapping mapping,
    IAccountService accountService,
    IReadFileService readFileService,
    INotificationService notificationService,
    ISupervisorAssignmentService supervisorAssignmentService,
    EventManager eventManager,
    BulkUpsertProgressSubject bulkUpsertProgressSubject,
    ICacheService cacheService
) : IShopService
{
    public async Task<Shop> CreateShop(CreateOrUpdateShopDto shopDto)
    {
        await unitOfWork.BeginTransaction();

        var account = accountService.GetCurrentAccount();
        if (!account.BrandId.HasValue)
            throw new BadRequestException(
                "Cannot create shop because current account doesn't have any registered Brand"
            );
        await IsValidShopDto(shopDto);

        var shop = mapping.Map<CreateOrUpdateShopDto, Shop>(shopDto);
        shop.ShopManagerId = null;
        shop.ShopStatus = ShopStatus.Active;
        shop.BrandId = account.BrandId.Value;
        shop = await unitOfWork.Shops.AddAsync(shop);
        await AssignShopManager(shop, shopDto.ShopManagerId);

        await unitOfWork.CommitTransaction();
        return shop;
    }

    public async Task DeleteShop(Guid id)
    {
        var installedEdgeBoxes = await edgeBoxService.GetEdgeBoxesByShop(id);
        if (installedEdgeBoxes.Any())
            throw new BadRequestException("Cannot delete shop that has active edge box");

        var shop = await GetShopById(id);
        if (
            (await unitOfWork.EdgeBoxInstalls.GetAsync(x => x.ShopId == id, takeAll: true)).Values is
            { Count: > 0 } installs
        )
        {
            if (installs.Any(i => i.EdgeBoxInstallStatus != EdgeBoxInstallStatus.Disabled))
                throw new BadRequestException("Cannot delete shop that currently has installed edge boxes");
            shop.ShopStatus = ShopStatus.Inactive;
        }
        else
        {
            unitOfWork.Shops.Delete(shop);
            unitOfWork.Employees.DeleteEmployeeInShop(id);
        }
        await unitOfWork.CompleteAsync();
        logger.Info($"Shop{shop.Id} has been Inactivated");
    }

    public async Task<Shop> GetShopById(Guid id)
    {
        var foundShop = await unitOfWork.Shops.GetAsync(new ShopByIdRepoSpec(id));
        var notFoundException = new NotFoundException(typeof(Shop), id);
        if (foundShop.Values.Count == 0)
            throw notFoundException;
        var account = accountService.GetCurrentAccount();
        var shop = foundShop.Values[0];
        if (account.Role == Role.Admin)
            return shop;
        if (account.Role == Role.BrandManager)
            return account.Brand != null && shop.BrandId == account.Brand.Id ? shop : throw notFoundException;
        if (account.Role == Role.ShopManager && shop.ShopManagerId == account.Id)
            return shop;
        throw notFoundException;
    }

    public async Task<PaginationResult<Shop>> GetShops(ShopSearchRequest searchRequest)
    {
        var includeWard = false;
        var account = accountService.GetCurrentAccount();
        if (account.Role == Role.BrandManager)
            searchRequest.BrandId = account.BrandId;
        else if (account.Role == Role.ShopManager)
        {
            searchRequest.ShopManagerId = account.Id;
            searchRequest.Status = ShopStatus.Active;
            includeWard = true;
        }

        var shops = await unitOfWork.Shops.GetAsync(new ShopSearchSpec(searchRequest, includeWard));
        return shops;
    }

    public async Task<PaginationResult<Shop>> GetShopsInstallingEdgeBox(bool hasEdgeBoxInstalling)
    {
        var installingEdgeBoxShops = (
            await unitOfWork.Shops.GetAsync(
                s =>
                    s.EdgeBoxInstalls.Any(i => i.EdgeBoxInstallStatus != EdgeBoxInstallStatus.Disabled)
                    == hasEdgeBoxInstalling,
                takeAll: true
            )
        ).Values.ToList();
        return new PaginationResult<Shop>
        {
            Values = installingEdgeBoxShops,
            PageIndex = 0,
            PageSize = installingEdgeBoxShops.Count,
            TotalCount = installingEdgeBoxShops.Count
        };
    }

    public async Task<Shop> UpdateShop(Guid id, CreateOrUpdateShopDto shopDto)
    {
        var foundShop =
            (await unitOfWork.Shops.GetAsync(new ShopByIdRepoSpec(id, false))).Values.FirstOrDefault()
            ?? throw new NotFoundException(typeof(Shop), id);
        var currentAccount = accountService.GetCurrentAccount();
        if (!(currentAccount.BrandId.HasValue && foundShop.BrandId == currentAccount.BrandId.Value))
            throw new ForbiddenException("Current user not allowed to do this action.");
        if (foundShop.ShopStatus != ShopStatus.Active)
            throw new BadRequestException("Cannot modified inactive shop");
        await IsValidShopDto(shopDto, id);

        var oldShopManagerId = foundShop.ShopManagerId;
        mapping.Map(shopDto, foundShop);
        foundShop.ShopManagerId = oldShopManagerId;
        if (await unitOfWork.CompleteAsync() > 0)
        {
            await AssignShopManager(foundShop, shopDto.ShopManagerId);
            eventManager.NotifyShopChanged(foundShop);
        }
        return foundShop;
    }

    public async Task<Shop> UpdateShopStatus(Guid shopId, ShopStatus shopStatus)
    {
        var foundShop = await unitOfWork.Shops.GetByIdAsync(shopId);
        if (foundShop == null)
            throw new NotFoundException(typeof(Shop), shopId);

        var currentAccount = accountService.GetCurrentAccount();
        var isBrandManager = currentAccount.BrandId.HasValue && foundShop.BrandId == currentAccount.BrandId.Value;
        if (!isBrandManager)
            throw new ForbiddenException("Current user not allowed to do this action.");
        // If shop's status is Inactive, only admin can update status
        if (foundShop.ShopStatus != ShopStatus.Active && currentAccount.Role != Role.Admin)
            throw new BadRequestException("Cannot update inactive shop");
        foundShop.ShopStatus = shopStatus;
        await unitOfWork.CompleteAsync();
        return await GetShopById(shopId);
    }

    private async Task IsValidShopDto(CreateOrUpdateShopDto shopDto, Guid? shopId = null)
    {
        if (!await unitOfWork.Wards.IsExisted(shopDto.WardId))
            throw new NotFoundException(typeof(Ward), shopDto.WardId);
        var account =
            (
                await unitOfWork.Accounts.GetAsync(
                    expression: a =>
                        a.Id == shopDto.ShopManagerId
                        && (a.AccountStatus == AccountStatus.Active || a.AccountStatus == AccountStatus.New),
                    includeProperties: [nameof(Account.ManagingShop)]
                )
            ).Values.FirstOrDefault() ?? throw new NotFoundException(typeof(Account), shopDto.ShopManagerId);
        if (account.Role != Role.ShopManager)
            throw new BadRequestException("Account is not a shop manager");

        var brandManager = accountService.GetCurrentAccount();
        if (account.BrandId != brandManager.BrandId)
            throw new BadRequestException(
                $"Account is not in the same brand as brand manager. Id {brandManager.BrandId}"
            );
    }

    private async Task AssignShopManager(Shop shop, Guid shopManagerId)
    {
        if (shop.ShopManagerId == shopManagerId)
            return;

        var shopManager =
            (
                await unitOfWork.Accounts.GetAsync(
                    a => a.Id == shopManagerId,
                    includeProperties: [nameof(Account.ManagingShop)]
                )
            ).Values.FirstOrDefault() ?? throw new NotFoundException(typeof(Account), shopManagerId);
        if (shopManager.Role != Role.ShopManager)
            throw new BadRequestException("Account is not a shop manager");

        if (shopManager.ManagingShop != null)
        {
            var oldShop =
                await unitOfWork.Shops.GetByIdAsync(shopManager.ManagingShop.Id)
                ?? throw new NotFoundException(typeof(Shop), shopManager.ManagingShop.Id);
            oldShop.ShopManagerId = null;
            unitOfWork.Shops.Update(oldShop);
        }
        shop.ShopManagerId = shopManagerId;
        unitOfWork.Shops.Update(shop);
        await unitOfWork.CompleteAsync();
    }

    public async Task<SupervisorAssignment> AssignSupervisorRoles(Guid accountId, Role role)
    {
        var account =
            await unitOfWork.Accounts.GetByIdAsync(accountId)
            ?? throw new NotFoundException(typeof(Account), accountId);
        switch (role)
        {
            case Role.ShopSupervisor:
                return await AssignSupervisor(account);
            default:
                throw new ArgumentOutOfRangeException(nameof(role), role, null);
        }
    }

    public async Task<SupervisorAssignment> AssignSupervisorRolesFromEmployee(Guid employeeId, Role role)
    {
        var employee =
            await unitOfWork.Employees.GetByIdAsync(employeeId)
            ?? throw new NotFoundException(typeof(Employee), employeeId);

        if (employee.Email == null)
            throw new BadRequestException("Employee email must not be empty");
        var accountId =
            employee.AccountId
            ?? (
                await accountService.CreateSupervisor(
                    new CreateSupervisorDto { EmployeeId = employeeId, Email = employee.Email }
                )
            ).Id;
        return await AssignSupervisorRoles(accountId, role);
    }

    public async Task<SupervisorAssignment> AssignHeadSupervisor(Account account)
    {
        var user = accountService.GetCurrentAccount();
        if (user.Role is not (Role.ShopManager or Role.SystemHandler))
            throw new BadRequestException("User is not a shop manager or system handler");
        var employee = (
            await unitOfWork.Employees.GetAsync(
                e => e.AccountId == account.Id,
                includeProperties: [nameof(Employee.Shop)]
            )
        ).Values.FirstOrDefault();
        if (employee?.ShopId == null)
            throw new BadRequestException("Invalid employee account");

        var currentTime = DateTimeHelper.VNDateTime;
        var latestAsm = await supervisorAssignmentService.GetLatestHeadSupervisorAssignmentByDate(
            employee.ShopId.Value,
            currentTime
        );
        var currentHeadSupervisor = latestAsm?.HeadSupervisor;
        var isShopOpening = IsShopOpeningAtTime(employee.Shop!, TimeOnly.FromDateTime(currentTime));

        if (latestAsm != null)
        {
            if (isShopOpening)
            {
                if (currentHeadSupervisor?.Id == account.Id)
                    return latestAsm;

                if (currentTime - latestAsm.StartTime < TimeSpan.FromMinutes(5))
                {
                    latestAsm.HeadSupervisorId = account.Id;
                    unitOfWork.SupervisorAssignments.Update(latestAsm);
                    await unitOfWork.CompleteAsync();
                    return latestAsm;
                }

                latestAsm.EndTime = currentTime;
                unitOfWork.SupervisorAssignments.Update(latestAsm);
            }
            else
                unitOfWork.SupervisorAssignments.Delete(latestAsm);

            if (currentHeadSupervisor != null && currentHeadSupervisor.Id != account.Id)
            {
                currentHeadSupervisor.Role = Role.ShopSupervisor;
                unitOfWork.Accounts.Update(currentHeadSupervisor);
            }
        }

        var supervisorAssignment = new SupervisorAssignment
        {
            ShopId = employee.ShopId,
            HeadSupervisorId = account.Id,
            StartTime = isShopOpening ? currentTime : GetNextOpenTime(employee.Shop!),
            EndTime = GetNextCloseTime(employee.Shop!)
        };
        await unitOfWork.SupervisorAssignments.AddAsync(supervisorAssignment);

        if (account.Role != Role.ShopHeadSupervisor)
        {
            account.Role = Role.ShopHeadSupervisor;
            unitOfWork.Accounts.Update(account);
        }

        await unitOfWork.CompleteAsync();
        return supervisorAssignment;
    }

    public async Task<SupervisorAssignment> AssignSupervisor(Account account)
    {
        var employee = (
            await unitOfWork.Employees.GetAsync(
                e => e.AccountId == account.Id,
                includeProperties: [nameof(Employee.Shop)]
            )
        ).Values.FirstOrDefault();
        if (employee?.ShopId == null)
            throw new BadRequestException("Invalid employee account");

        var currentTime = DateTimeHelper.VNDateTime;
        var isShopOpening = IsShopOpeningAtTime(employee.Shop!, TimeOnly.FromDateTime(currentTime));
        var latestAsm = await supervisorAssignmentService.GetLatestAssignment(employee.ShopId.Value);
        var currentSupervisor = latestAsm?.Supervisor;

        if (latestAsm != null)
        {
            if (isShopOpening)
            {
                if (currentSupervisor?.Id == account.Id)
                    return latestAsm;

                if (currentTime - latestAsm.StartTime < TimeSpan.FromMinutes(5))
                {
                    latestAsm.SupervisorId = account.Id;
                    unitOfWork.SupervisorAssignments.Update(latestAsm);

                    // assign new in charge account to all incidents
                    var incidents = (
                        await unitOfWork.Incidents.GetAsync(
                            i => latestAsm.StartTime <= i.StartTime && i.StartTime <= currentTime,
                            takeAll: true
                        )
                    ).Values;
                    foreach (var incident in incidents)
                    {
                        incident.InChargeAccountId = account.Id;
                        unitOfWork.Incidents.Update(incident);
                    }

                    await unitOfWork.CompleteAsync();
                    return latestAsm;
                }

                latestAsm.EndTime = currentTime;
                unitOfWork.SupervisorAssignments.Update(latestAsm);
            }
            else
                unitOfWork.SupervisorAssignments.Delete(latestAsm);
        }

        var supervisorAssignment = new SupervisorAssignment
        {
            ShopId = employee.ShopId,
            SupervisorId = account.Id,
            StartTime = isShopOpening ? currentTime : GetNextOpenTime(employee.Shop!),
            EndTime = GetNextCloseTime(employee.Shop!)
        };
        await unitOfWork.SupervisorAssignments.AddAsync(supervisorAssignment);

        if (account.Role != Role.ShopSupervisor)
        {
            account.Role = Role.ShopSupervisor;
            unitOfWork.Accounts.Update(account);
        }

        await unitOfWork.CompleteAsync();
        return supervisorAssignment;
    }

    public static bool IsShopOpeningAtTime(Shop shop, TimeOnly time)
    {
        if (shop.OpenTime < shop.CloseTime)
            return shop.OpenTime <= time && time < shop.CloseTime;
        return shop.OpenTime <= time || time < shop.CloseTime;
    }

    public static DateTime GetNextOpenTime(Shop shop)
    {
        var currentDateTime = DateTimeHelper.VNDateTime;
        var currentTime = TimeOnly.FromDateTime(currentDateTime);
        return new DateTime(
            currentTime < shop.OpenTime
                ? DateOnly.FromDateTime(currentDateTime)
                : DateOnly.FromDateTime(currentDateTime).AddDays(1),
            shop.OpenTime,
            DateTimeKind.Unspecified
        );
    }

    public static DateTime GetNextCloseTime(Shop shop)
    {
        var currentDateTime = DateTimeHelper.VNDateTime;
        var currentTime = TimeOnly.FromDateTime(currentDateTime);
        return new DateTime(
            currentTime < shop.CloseTime
                ? DateOnly.FromDateTime(currentDateTime)
                : DateOnly.FromDateTime(currentDateTime).AddDays(1),
            shop.CloseTime,
            DateTimeKind.Unspecified
        );
    }

    public static DateTime GetLastOpenTime(Shop shop)
    {
        var currentDateTime = DateTimeHelper.VNDateTime;
        var currentTime = TimeOnly.FromDateTime(currentDateTime);
        return new DateTime(
            currentTime < shop.OpenTime
                ? DateOnly.FromDateTime(currentDateTime).AddDays(-1)
                : DateOnly.FromDateTime(currentDateTime),
            shop.OpenTime,
            DateTimeKind.Unspecified
        );
    }

    public async Task<BulkUpsertTaskResultResponse> UpsertShops(Guid actorId, Stream stream, string taskId)
    {
        var shopInserted = new HashSet<Guid>();
        var shopUpdated = new HashSet<Guid>();
        var accountInserted = new HashSet<Guid>();
        var accountUpdated = new HashSet<Guid>();
        var failedValidatedRecords = new Dictionary<int, object?>();
        var rowCount = 1;
        var brand =
            (await unitOfWork.Brands.GetAsync(expression: b => b.BrandManagerId == actorId)).Values.FirstOrDefault()
            ?? throw new NotFoundException("Cannot find brand manager when upsert");
        await unitOfWork.BeginTransaction();
        try
        {
            foreach (
                var record in readFileService.ReadFromCsv<ShopFromImportFile>(
                    stream,
                    $"failed-records-{taskId}",
                    true,
                    $"total-records-{taskId}"
                )
            )
            {
                bulkUpsertProgressSubject.Notify(new(rowCount++, taskId));
                if (record is null)
                {
                    failedValidatedRecords.Add(rowCount, new { Failed = "Cannot parse record" });
                    continue;
                }
                if (!record.IsValid())
                {
                    failedValidatedRecords.Add(rowCount, record.ShopFromImportFileValidation());
                    continue;
                }

                var shop = (
                    await unitOfWork.Shops.GetAsync(
                        expression: s => s.ExternalId == record.GetShop().ExternalId,
                        disableTracking: false
                    )
                ).Values.FirstOrDefault();
                var account = (
                    await unitOfWork.Accounts.GetAsync(
                        expression: a =>
                            a.ExternalId == record.GetManager().ExternalId || a.Email == record.GetManager().Email,
                        disableTracking: false
                    )
                ).Values.FirstOrDefault();
                if (account == null)
                {
                    account = record.GetManager();
                    account.Password = Hasher.Hash(DomainHelper.GenerateDefaultPassword(account.Email));
                    account.BrandId = brand.Id;
                    account.Role = Role.ShopManager;
                    account.AccountStatus = AccountStatus.New;
                    account = await unitOfWork.Accounts.AddAsync(account);
                    accountInserted.Add(account.Id);
                }
                else
                {
                    account.Email = record.GetManager().Email;
                    account.Name = record.GetManager().Name;
                    account.Gender = record.GetManager().Gender;
                    account.AddressLine = record.GetManager().AddressLine;
                    account.ExternalId = record.GetManager().ExternalId;
                    account.BrandId = brand.Id;
                    account.Role = Role.ShopManager;
                    unitOfWork.Accounts.Update(account);
                    accountUpdated.Add(account.Id);
                }

                if (shop == null)
                {
                    shop = record.GetShop();
                    shop.ShopManagerId = account.Id;
                    shop.BrandId = brand.Id;
                    shop.ShopStatus = ShopStatus.Active;
                    shop = await unitOfWork.Shops.AddAsync(shop);
                    shopInserted.Add(shop.Id);
                }
                else
                {
                    shop.ExternalId = record.GetShop().ExternalId;
                    shop.Name = record.GetShop().Name;
                    shop.OpenTime = record.GetShop().OpenTime;
                    shop.CloseTime = record.GetShop().CloseTime;
                    shop.Phone = record.GetShop().Phone;
                    shop.AddressLine = record.GetShop().AddressLine;
                    shop.BrandId = brand.Id;
                    unitOfWork.Shops.Update(shop);
                    shopUpdated.Add(shop.Id);
                }
            }

            await unitOfWork.CompleteAsync();
            await unitOfWork.CommitTransaction();
            var totalOfInserted = shopInserted.Count + accountInserted.Count;
            var totalOfUpdated = shopUpdated.Count + accountUpdated.Count;
            await notificationService.CreateNotification(
                new(taskId)
                {
                    Priority = NotificationPriority.Normal,
                    Content =
                        $"Inserted: {totalOfInserted}\nUpdated: {totalOfUpdated}\nFailed:{failedValidatedRecords.Count}",
                    Title = "Upsert employees completed",
                    Type = NotificationType.UpsertShopAndManager,
                    SentToId = [actorId],
                }
            );
            var result = new BulkUpsertTaskResultResponse(
                BulkUpsertStatus.Success,
                totalOfInserted,
                totalOfUpdated,
                failedValidatedRecords.Count,
                "",
                new { ShopInserted = shopInserted },
                new { AccountInserted = accountInserted },
                new { ShopUpdated = shopUpdated },
                new { AccountUpdated = accountUpdated },
                new { Errors = failedValidatedRecords.Select(e => new { Row = e.Key, Reasons = e.Value }) },
                new
                {
                    UnhandledErrors = cacheService.Get<List<string>>($"failed-records-{taskId}", isRemoveAfterGet: true)
                        ?? new List<string>()
                }
            );
            return result;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
            await unitOfWork.RollBack();
            await notificationService.CreateNotification(
                new(taskId)
                {
                    Priority = NotificationPriority.Urgent,
                    Content = "Upsert failed",
                    Type = NotificationType.UpsertShopAndManager,
                    SentToId = [actorId],
                    Title = "Upsert Failed",
                }
            );
        }
        finally
        {
            stream.Close();
        }
        return new BulkUpsertTaskResultResponse(BulkUpsertStatus.Fail, 0, 0, 0, "Shop and shop manager upsert failed");
    }

    public async Task<bool> IsInCharge()
    {
        var user = accountService.GetCurrentAccount();
        var shopId = user.ManagingShop?.Id;
        if (shopId == null)
        {
            var employee = (
                await unitOfWork.Employees.GetAsync(
                    e => e.AccountId == user.Id,
                    includeProperties: [nameof(Employee.Shop)]
                )
            ).Values.FirstOrDefault();
            if (employee?.ShopId == null)
                throw new BadRequestException("Invalid employee account");
            shopId = employee.ShopId;
        }

        var latestAsm = await supervisorAssignmentService.GetLatestAssignment(shopId.Value);
        var inChargeId = latestAsm?.SupervisorId;
        if (inChargeId == null)
            return user.Role == Role.ShopManager;
        return inChargeId == user.Id;
    }
}
