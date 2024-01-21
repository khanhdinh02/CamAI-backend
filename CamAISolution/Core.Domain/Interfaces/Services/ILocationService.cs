using Core.Domain.Entities;

namespace Core.Domain.Services;

public interface ILocationService
{
    Task<IEnumerable<Province>> GetAllProvinces();
    Task<IEnumerable<District>> GetAllDistrictsByProvinceId(int provinceId);
    Task<IEnumerable<Ward>> GetAllWardsByDistrictId(int districtId);
}
