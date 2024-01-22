using Core.Application.Exceptions;
using Core.Domain.Entities;
using Core.Domain.Interfaces.Services;
using Core.Domain.Repositories;

namespace Core.Application.Implements;

public class LocationService(IUnitOfWork unitOfWork) : ILocationService
{
    public async Task<IEnumerable<District>> GetAllDistrictsByProvinceId(int provinceId)
    {
        var province =
            (
                await unitOfWork
                    .Provinces
                    .GetAsync(p => p.Id == provinceId, includeProperties: [nameof(Province.Districts)])
            )
                .Values
                .FirstOrDefault() ?? throw new NotFoundException(typeof(Province), provinceId);
        return province.Districts;
    }

    public async Task<IEnumerable<Province>> GetAllProvinces()
    {
        var allProvinces = await unitOfWork.Provinces.GetAsync(takeAll: true);
        return allProvinces.Values;
    }

    public async Task<IEnumerable<Ward>> GetAllWardsByDistrictId(int districtId)
    {
        var district =
            (
                await unitOfWork
                    .Districts
                    .GetAsync(
                        d => d.Id == districtId,
                        includeProperties: [nameof(District.Wards), nameof(District.Province)]
                    )
            )
                .Values
                .FirstOrDefault() ?? throw new NotFoundException(typeof(District), districtId);
        return district.Wards;
    }
}
