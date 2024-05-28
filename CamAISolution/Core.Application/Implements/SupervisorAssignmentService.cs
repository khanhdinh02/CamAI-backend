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
    public async Task<SupervisorAssignment?> GetLatestAssignmentByDate(
        Guid shopId,
        DateTime date,
        bool includeAll = true
    )
    {
        var shop = await unitOfWork.Shops.GetByIdAsync(shopId) ?? throw new NotFoundException(typeof(Shop), shopId);
        var openTime = date.Date.Add(shop.OpenTime.ToTimeSpan());
        var closeTime = openTime.AddDays(1);
        if (includeAll)
            return (
                await unitOfWork.SupervisorAssignments.GetAsync(
                    a => a.ShopId == shopId && openTime <= a.StartTime && a.StartTime < closeTime,
                    includeProperties: [nameof(SupervisorAssignment.Supervisor)],
                    orderBy: o => o.OrderByDescending(x => x.StartTime),
                    pageSize: 1
                )
            ).Values.FirstOrDefault();
        else
            return (
                await unitOfWork.SupervisorAssignments.GetAsync(
                    a => a.ShopId == shopId && openTime <= a.StartTime && a.StartTime < closeTime,
                    orderBy: o => o.OrderByDescending(x => x.StartTime),
                    pageSize: 1
                )
            ).Values.FirstOrDefault();
    }

    public async Task<Account?> GetCurrentInChangeAccount(Guid shopId)
    {
        var shop = await unitOfWork.Shops.GetByIdAsync(shopId) ?? throw new NotFoundException(typeof(Shop), shopId);
        var assignment = await GetLatestAssignmentByDate(shopId, ShopService.GetLastOpenTime(shop));
        return assignment?.Supervisor
            ?? (await unitOfWork.Accounts.GetAsync(a => a.ManagingShop!.Id == shopId)).Values.FirstOrDefault();
    }

    public async Task<IList<SupervisorAssignment>> GetSupervisorAssignmentByDate(DateTime date)
    {
        var startTime = date.Date;
        var endTime = startTime.AddDays(1).AddTicks(-1);
        var account = accountService.GetCurrentAccount();
        Expression<Func<SupervisorAssignment, bool>> criteria = account.Role switch
        {
            Role.ShopManager
                => x =>
                    (
                        (startTime <= x.StartTime && x.StartTime <= endTime)
                        || (startTime <= x.EndTime && x.EndTime <= endTime)
                    )
                    && x.ShopId == account.ManagingShop!.Id,
            Role.ShopSupervisor
                => x =>
                    (
                        (startTime <= x.StartTime && x.StartTime <= endTime)
                        || (startTime <= x.EndTime && x.EndTime <= endTime)
                    )
                    && x.SupervisorId == account.Id,
            _ => throw new ForbiddenException("Cannot get supervisor assignments")
        };

        return (
            await unitOfWork.SupervisorAssignments.GetAsync(
                criteria,
                orderBy: x => x.OrderBy(e => e.StartTime),
                includeProperties: [nameof(SupervisorAssignment.Supervisor)]
            )
        ).Values;
    }

    public async Task RemoveSupervisor()
    {
        var account = accountService.GetCurrentAccount();
        var shop = account.ManagingShop;

        if (account.Role != Role.ShopManager || shop == null)
            throw new ForbiddenException("Account is not shop manager");

        var now = DateTimeHelper.VNDateTime;
        var isShopOpening = ShopService.IsShopOpeningAtTime(shop, TimeOnly.FromDateTime(now));
        var assignments = (
            await GetSupervisorAssignmentByDate(
                isShopOpening ? ShopService.GetLastOpenTime(shop) : ShopService.GetNextOpenTime(shop)
            )
        )
            .OrderByDescending(x => x.StartTime)
            .ToList();
        var latestAssignment =
            assignments.Count > 0 ? assignments[0] : throw new ForbiddenException("Shop does not have any assignment");
        var prevAssignment = assignments.Count > 1 ? assignments[1] : null;

        if (isShopOpening)
        {
            if (now - latestAssignment.StartTime < TimeSpan.FromMinutes(5))
            {
                latestAssignment.SupervisorId = shop.ShopManagerId;
                if (prevAssignment != null && prevAssignment.SupervisorId == latestAssignment.SupervisorId)
                {
                    // merge latest assignment with previous assignment if both of them have the same supervisor
                    prevAssignment.EndTime = latestAssignment.EndTime;
                    prevAssignment.Supervisor = null;
                    unitOfWork.Incidents.UpdateIncidentAssignment(latestAssignment.Id, prevAssignment.Id);
                    unitOfWork.SupervisorAssignments.Update(prevAssignment);
                    unitOfWork.SupervisorAssignments.Delete(latestAssignment);
                }
                else
                    unitOfWork.SupervisorAssignments.Update(latestAssignment);
            }
            else
            {
                latestAssignment.EndTime = now;
                unitOfWork.SupervisorAssignments.Update(latestAssignment);
                var newAssignment = new SupervisorAssignment
                {
                    ShopId = shop.Id,
                    StartTime = latestAssignment.EndTime.Value,
                    EndTime = ShopService.GetNextCloseTime(shop),
                    SupervisorId = shop.ShopManagerId,
                };
                await unitOfWork.SupervisorAssignments.AddAsync(newAssignment);
            }
        }
        else
        {
            var nextOpenTime = ShopService.GetNextOpenTime(shop);
            if (nextOpenTime <= latestAssignment.StartTime)
            {
                latestAssignment.SupervisorId = shop.ShopManagerId;
                if (prevAssignment != null && prevAssignment.SupervisorId == latestAssignment.SupervisorId)
                {
                    prevAssignment.EndTime = latestAssignment.EndTime;
                    // TODO: update incident assignment id
                    unitOfWork.SupervisorAssignments.Update(prevAssignment);
                    unitOfWork.SupervisorAssignments.Delete(latestAssignment);
                }
                else
                {
                    unitOfWork.SupervisorAssignments.Update(latestAssignment);
                }
            }
            else
            {
                var newAssignment = new SupervisorAssignment
                {
                    ShopId = shop.Id,
                    SupervisorId = shop.ShopManagerId,
                    StartTime = ShopService.GetNextOpenTime(shop),
                    EndTime = ShopService.GetNextCloseTime(shop)
                };
                await unitOfWork.SupervisorAssignments.AddAsync(newAssignment);
            }
        }

        await unitOfWork.CompleteAsync();
    }
}
