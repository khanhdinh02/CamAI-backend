using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Repositories;
using Core.Domain.Services;
using Core.Domain.Utilities;

namespace Core.Application.Implements;

public class SupervisorAssignmentService(IUnitOfWork unitOfWork) : ISupervisorAssignmentService
{
    public async Task<SupervisorAssignment?> GetLatestSupervisorAssignment(Guid shopId)
    {
        return (await unitOfWork.SupervisorAssignments.GetAsync(
            a =>
                a.ShopId == shopId
                && a.StartTime.Date == DateTimeHelper.VNDateTime.Date
                && a.AssigneeRole == Role.ShopSupervisor,
            includeProperties: [nameof(SupervisorAssignment.Assignee)],
            orderBy: o => o.OrderByDescending(x => x.StartTime),
            pageSize: 1
        )).Values.FirstOrDefault();
    }

    public async Task<SupervisorAssignment?> GetLatestHeadSupervisorAssignment(Guid shopId)
    {
        return (await unitOfWork.SupervisorAssignments.GetAsync(
            a =>
                a.ShopId == shopId
                && a.StartTime.Date == DateTimeHelper.VNDateTime.Date
                && a.AssigneeRole == Role.ShopHeadSupervisor,
            includeProperties: [nameof(SupervisorAssignment.Assignee)],
            orderBy: o => o.OrderByDescending(x => x.StartTime),
            pageSize: 1
        )).Values.FirstOrDefault();
    }
}
