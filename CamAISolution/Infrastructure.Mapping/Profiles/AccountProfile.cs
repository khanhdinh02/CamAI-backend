using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Infrastructure.Mapping.Profiles;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        CreateMap<CreateAccountDto, Account>();
        CreateMap<UpdateAccountDto, Account>();
        CreateMap<Account, AccountDto>()
            .ForMember(member => member.Roles, options => options.MapFrom(src => src.Roles.Select(ar => ar.Role)));
        CreateMap<Account, AccountDtoWithoutBrand>()
            .ForMember(member => member.Roles, options => options.MapFrom(src => src.Roles.Select(ar => ar.Role)));
        CreateMap<UpdateProfileDto, Account>();
    }
}
