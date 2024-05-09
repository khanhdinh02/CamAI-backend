using Core.Domain.Entities;
using Core.Domain.Repositories;
using Core.Domain.Services;
using Core.Domain.Utilities;

namespace Core.Application.Implements;

public class SupervisorAssignmentService(IUnitOfWork unitOfWork) : ISupervisorAssignmentService
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
}
