using Core.Application.Exceptions;
using Core.Domain.Entities;
using Core.Domain.Repositories;
using Core.Domain.Services;

namespace Core.Application.Implements;

public class LocationService(IRepository<Province> provinces, IRepository<District> districts) : ILocationService
{
    public async Task<IEnumerable<District>> GetAllDistrictsByProvinceId(int provinceId)
    {
        var foundProvince = await provinces.GetAsync(
            expression: p => p.Id == provinceId,
            orderBy: o => o.OrderBy(p => p.Id),
            includeProperties: [nameof(Province.Districts)]
        );
        var province = foundProvince.Values.Any()
            ? foundProvince.Values[0]
            : throw new NotFoundException(typeof(Province), provinceId);
        return province.Districts;
    }

    public async Task<IEnumerable<Province>> GetAllProvinces()
    {
        var allProvinces = await provinces.GetAsync(takeAll: true);
        return allProvinces.Values;
    }

    public async Task<IEnumerable<Ward>> GetAllWardsByDistrictId(int districtId)
    {
        var foundDistrict = await districts.GetAsync(
            expression: d => d.Id == districtId,
            orderBy: o => o.OrderBy(d => d.Id),
            includeProperties: [nameof(District.Wards)]
        );
        var district = foundDistrict.Values.Any()
            ? foundDistrict.Values[0]
            : throw new NotFoundException(typeof(District), districtId);
        return district.Wards;
    }
}
