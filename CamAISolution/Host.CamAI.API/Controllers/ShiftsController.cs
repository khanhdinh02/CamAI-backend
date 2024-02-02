using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;
using Infrastructure.Jwt.Attribute;
using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShiftsController(IShiftService shiftService, IBaseMapping mapper) : ControllerBase
{
    [HttpGet]
    [AccessTokenGuard(RoleEnum.ShopManager, RoleEnum.BrandManager, RoleEnum.Admin)]
    public async Task<IEnumerable<ShiftDto>> Get(Guid? shopId)
    {
        return (await shiftService.GetShifts(shopId)).Select(mapper.Map<Shift, ShiftDto>);
    }

    [HttpPost]
    [AccessTokenGuard(RoleEnum.ShopManager)]
    public async Task<ShiftDto> Create(CreateShiftDto dto)
    {
        return mapper.Map<Shift, ShiftDto>(await shiftService.CreateShift(dto));
    }

    [HttpDelete("{id}")]
    [AccessTokenGuard(RoleEnum.ShopManager)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await shiftService.DeleteShift(id);
        return Accepted();
    }
}
