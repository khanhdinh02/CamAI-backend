using System.Linq.Expressions;
using Core.Application.Exceptions;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Services;
using Core.Domain.Repositories;
using Core.Domain.Services;
using Core.Domain.Utilities;

namespace Core.Application.Implements;

public class SupervisorAssignmentService(IAccountService accountService, IUnitOfWork unitOfWork)
    : ISupervisorAssignmentService
{
    public async Task<SupervisorAssignment?> GetLatestAssignmentByDate(Guid shopId, DateTime date)
    {
        var shop = await unitOfWork.Shops.GetByIdAsync(shopId) ?? throw new NotFoundException(typeof(Shop), shopId);
        var openTime = date.Date.Add(shop.OpenTime.ToTimeSpan());
        var closeTime = openTime.AddDays(1);
        return (
            await unitOfWork.SupervisorAssignments.GetAsync(
                a => a.ShopId == shopId && openTime <= a.StartTime && a.StartTime < closeTime,
                includeProperties:
                [
                    nameof(SupervisorAssignment.HeadSupervisor),
                    nameof(SupervisorAssignment.Supervisor)
                ],
                orderBy: o => o.OrderByDescending(x => x.StartTime),
                pageSize: 1
            )
        ).Values.FirstOrDefault();
    }

    public async Task<SupervisorAssignment?> GetLatestHeadSupervisorAssignmentByDate(Guid shopId, DateTime date)
    {
        var shop = await unitOfWork.Shops.GetByIdAsync(shopId) ?? throw new NotFoundException(typeof(Shop), shopId);
        var openTime = date.Date.Add(shop.OpenTime.ToTimeSpan());
        var closeTime = openTime.AddDays(1);
        return (
            await unitOfWork.SupervisorAssignments.GetAsync(
                a => a.ShopId == shopId && openTime <= a.StartTime && a.StartTime < closeTime && a.SupervisorId == null,
                includeProperties:
                [
                    nameof(SupervisorAssignment.HeadSupervisor),
                    nameof(SupervisorAssignment.Supervisor)
                ],
                orderBy: o => o.OrderByDescending(x => x.StartTime),
                pageSize: 1
            )
        ).Values.FirstOrDefault();
    }

    public async Task<Account?> GetCurrentInChangeAccount(Guid shopId)
    {
        var assignment = await GetLatestAssignmentByDate(shopId, DateTimeHelper.VNDateTime);
        return assignment?.Supervisor
            ?? assignment?.HeadSupervisor
            ?? (await unitOfWork.Accounts.GetAsync(a => a.ManagingShop!.Id == shopId)).Values.FirstOrDefault();
    }

    public async Task<Account?> GetCurrentInChangeHeadSupervisorAccount(Guid shopId)
    {
        var assignment = await GetLatestAssignmentByDate(shopId, DateTimeHelper.VNDateTime);
        return assignment?.HeadSupervisor;
    }

    public async Task<IList<SupervisorAssignment>> GetSupervisorAssignmentByDate(DateTime date)
    {
        var startTime = date.Date;
        var endTime = startTime.AddDays(1).AddTicks(-1);
        var account = accountService.GetCurrentAccount();
        Expression<Func<SupervisorAssignment, bool>> criteria = account.Role switch
        {
            Role.ShopManager => x => startTime <= x.StartTime && x.StartTime <= endTime,
            Role.ShopHeadSupervisor
            or Role.ShopSupervisor
                => x =>
                    startTime <= x.StartTime
                    && x.StartTime <= endTime
                    && (x.HeadSupervisorId == account.Id || x.SupervisorId == account.Id),
            _ => throw new ForbiddenException("Cannot get supervisor assignments")
        };

        return (
            await unitOfWork.SupervisorAssignments.GetAsync(
                criteria,
                orderBy: x => x.OrderBy(e => e.StartTime),
                includeProperties: ["HeadSupervisor", "Supervisor"]
            )
        ).Values;
    }

    public async Task RemoveHeadSupervisor()
    {
        var account = accountService.GetCurrentAccount();
        var shop = account.ManagingShop!;
        if (ShopService.IsShopOpeningAtTime(shop, TimeOnly.FromDateTime(DateTime.Now)))
        {
            var latestAssignment = await GetLatestAssignmentByDate(shop.Id, DateTime.Now);
            if (latestAssignment == null)
                return;

            var now = DateTimeHelper.VNDateTime;
            if (now - latestAssignment.StartTime < TimeSpan.FromMinutes(5))
            {
                latestAssignment.HeadSupervisorId = null;
                latestAssignment.SupervisorId = null;
                unitOfWork.SupervisorAssignments.Update(latestAssignment);
                await unitOfWork.CompleteAsync();
            }
            else
            {
                latestAssignment.EndTime = now;
                unitOfWork.SupervisorAssignments.Update(latestAssignment);
                var newAssignment = new SupervisorAssignment
                {
                    ShopId = shop.Id,
                    StartTime = latestAssignment.EndTime.Value
                };
                await unitOfWork.SupervisorAssignments.AddAsync(newAssignment);
                await unitOfWork.CompleteAsync();
            }
        }
        else
        {
            var newAssignment = new SupervisorAssignment
            {
                ShopId = shop.Id,
                StartTime = ShopService.GetNextOpenTime(shop)
            };
            await unitOfWork.SupervisorAssignments.AddAsync(newAssignment);
            await unitOfWork.CompleteAsync();
        }
    }

    public async Task RemoveSupervisor()
    {
        var account = accountService.GetCurrentAccount();
        var employee = (
            await unitOfWork.Employees.GetAsync(x => x.AccountId == account.Id, includeProperties: ["Shop"])
        ).Values[0];
        var shop = employee.Shop!;
        var latestAssignment = await GetLatestAssignmentByDate(shop.Id, DateTime.Now);
        if (latestAssignment == null)
            throw new ForbiddenException("Shop does not have any assignment");
        if (latestAssignment.HeadSupervisorId != account.Id)
            throw new ForbiddenException("Account is not current head supervisor");

        if (ShopService.IsShopOpeningAtTime(shop, TimeOnly.FromDateTime(DateTime.Now)))
        {
            var now = DateTimeHelper.VNDateTime;
            if (now - latestAssignment.StartTime < TimeSpan.FromMinutes(5))
            {
                latestAssignment.SupervisorId = null;
                unitOfWork.SupervisorAssignments.Update(latestAssignment);
                await unitOfWork.CompleteAsync();
            }
            else
            {
                latestAssignment.EndTime = DateTimeHelper.VNDateTime;
                unitOfWork.SupervisorAssignments.Update(latestAssignment);
                var newAssignment = new SupervisorAssignment
                {
                    ShopId = shop.Id,
                    StartTime = latestAssignment.EndTime.Value,
                    HeadSupervisorId = account.Id
                };
                await unitOfWork.SupervisorAssignments.AddAsync(newAssignment);
                await unitOfWork.CompleteAsync();
            }
        }
        else
        {
            var newAssignment = new SupervisorAssignment
            {
                ShopId = shop.Id,
                StartTime = ShopService.GetNextOpenTime(shop),
                HeadSupervisorId = account.Id
            };
            await unitOfWork.SupervisorAssignments.AddAsync(newAssignment);
            await unitOfWork.CompleteAsync();
        }
    }
}
