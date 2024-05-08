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
            .ForMember(
                x => x.ManagingShop,
                opts =>
                    opts.MapFrom(
                        (account, _, _, ctx) =>
                        {
                            if (account.ManagingShop == null)
                                return null;

                            account.ManagingShop.ShopManager = null;
                            account.ManagingShop.Brand = null!;
                            return ctx.Mapper.Map<Shop, ShopDto>(account.ManagingShop);
                        }
                    )
            )
            .ForMember(
                x => x.Brand,
                opts =>
                    opts.MapFrom(
                        (account, _, _, ctx) =>
                        {
                            if (account.Brand == null)
                                return null;
                            account.Brand.BrandManager = null;
                            return ctx.Mapper.Map<Brand, BrandDto>(account.Brand);
                        }
                    )
            );
        CreateMap<UpdateProfileDto, Account>();
        CreateMap<Employee, Account>();
    }
}
