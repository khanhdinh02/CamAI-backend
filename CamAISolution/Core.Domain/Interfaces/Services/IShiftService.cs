using Core.Domain.DTO;
using Core.Domain.Entities;

namespace Core.Domain.Interfaces.Services;

public interface IShiftService
{
    Task<IEnumerable<Shift>> GetShifts(Guid? shopId);
    Task<Shift> CreateShift(CreateShiftDto dto);
    Task DeleteShift(Guid id);
}
