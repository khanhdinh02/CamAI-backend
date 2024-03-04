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
            .ForPath(x => x.ManagingShop!.ShopManager, opts => opts.Ignore())
            .ForPath(x => x.Brand!.BrandManager, opts => opts.Ignore());
        CreateMap<UpdateProfileDto, Account>();
    }
}
