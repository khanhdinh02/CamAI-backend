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
        return (
            await unitOfWork.SupervisorAssignments.GetAsync(
                a => a.ShopId == shopId && a.StartTime.Date == date.Date,
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
        return (
            await unitOfWork.SupervisorAssignments.GetAsync(
                a => a.ShopId == shopId && a.StartTime.Date == date.Date && a.SupervisorId == null,
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
}
