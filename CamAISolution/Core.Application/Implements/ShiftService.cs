using Core.Application.Exceptions;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Core.Domain.Repositories;
using Core.Domain.Utilities;

namespace Core.Application.Implements;

public class ShiftService(IUnitOfWork unitOfWork, IBaseMapping mapper, IAccountService accountService) : IShiftService
{
    public async Task<IEnumerable<Shift>> GetShifts(Guid? shopId)
    {
        var user = accountService.GetCurrentAccount();
        if (user.HasRole(RoleEnum.Admin))
        {
            if (!shopId.HasValue)
                throw new BadRequestException("ShopId is required");
        }
        else if (user.HasRole(RoleEnum.BrandManager))
        {
            if (!shopId.HasValue)
                throw new BadRequestException("ShopId is required");
            var shop = await unitOfWork.Shops.GetByIdAsync(shopId) ?? throw new NotFoundException(typeof(Shop), shopId);
            if (shop.BrandId != user.BrandId)
                throw new ForbiddenException(user, shop);
        }
        else if (user.HasRole(RoleEnum.ShopManager))
        {
            shopId = user.ManagingShop?.Id;
        }

        return (
            await unitOfWork
                .Shifts
                .GetAsync(s => s.ShopId == shopId, orderBy: s => s.OrderBy(k => k.StartTime), takeAll: true)
        ).Values;
    }

    public async Task<Shift> CreateShift(CreateShiftDto dto)
    {
        var user = accountService.GetCurrentAccount();
        if (user.ManagingShop == null)
            throw new ForbiddenException(user, typeof(Shop));

        var shift = mapper.Map<CreateShiftDto, Shift>(dto);
        shift.ShopId = user.ManagingShop.Id;
        await unitOfWork.Shifts.AddAsync(shift);
        await unitOfWork.CompleteAsync();
        return shift;
    }

    public async Task DeleteShift(Guid id)
    {
        var shift = await unitOfWork.Shifts.GetByIdAsync(id) ?? throw new NotFoundException(typeof(Shift), id);
        var user = accountService.GetCurrentAccount();
        if (user.ManagingShop?.Id != shift.ShopId)
            throw new ForbiddenException(user, shift.Shop);

        unitOfWork.Shifts.Delete(shift);
        await unitOfWork.CompleteAsync();
    }
}
