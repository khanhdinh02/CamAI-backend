using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Infrastructure.Mapping.Profiles;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<Employee, EmployeeDto>()
            .ForMember(
                x => x.EmployeeRole,
                opts =>
                {
                    opts.MapFrom<EmployeeRole?>(
                        (employee, _) =>
                        {
                            if (employee.AccountId == null)
                                return EmployeeRole.Employee;
                            return employee.Account?.Role switch
                            {
                                Role.ShopHeadSupervisor => EmployeeRole.HeadSupervisor,
                                Role.ShopSupervisor => EmployeeRole.Supervisor,
                                _ => null
                            };
                        }
                    );
                }
            );
        CreateMap<CreateEmployeeDto, Employee>();
        CreateMap<UpdateEmployeeDto, Employee>();
    }
}
