using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;

namespace Infrastructure.Mapping.Profiles;

public class SupervisorAssignmentProfile : Profile
{
    public SupervisorAssignmentProfile()
    {
        CreateMap<SupervisorAssignment, SupervisorAssignmentDto>();
    }

    public static async Task<SupervisorAssignmentDto> ToSupervisorAssignmentDto(
        IBaseMapping mapping,
        IAccountService accountService,
        IEmployeeService employeeService,
        SupervisorAssignment supervisorAssignment
    )
    {
        var dto = mapping.Map<SupervisorAssignment, SupervisorAssignmentDto>(supervisorAssignment);
        var inCharge =
            supervisorAssignment.Supervisor
            ?? supervisorAssignment.HeadSupervisor
            ?? accountService.GetCurrentAccount();
        Role inChargeRole;
        if (supervisorAssignment.Supervisor != null)
            inChargeRole = Role.ShopSupervisor;
        else if (supervisorAssignment.HeadSupervisor != null)
            inChargeRole = Role.ShopHeadSupervisor;
        else
            inChargeRole = Role.ShopManager;
        dto.InChargeAccount = mapping.Map<Account, AccountDto>(inCharge);
        dto.InChargeAccountId = inCharge.Id;
        dto.InChargeAccountRole = inChargeRole;
        var employee = await employeeService.GetEmployeeAccount(inCharge.Id);
        dto.InChargeEmployeeId = employee?.Id;
        return dto;
    }
}
