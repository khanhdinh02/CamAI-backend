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
        CreateMap<Account, AccountDtoWithBrand>();
        CreateMap<Account, AccountDtoWithShop>();
        CreateMap<Account, AccountDtoWithoutBrandAndShop>();
        CreateMap<UpdateProfileDto, Account>();
    }
}
