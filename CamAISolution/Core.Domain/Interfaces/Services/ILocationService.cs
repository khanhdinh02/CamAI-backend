﻿using Core.Domain.Entities;

namespace Core.Domain.Services;

public interface ILocationService
{
    Task<IEnumerable<Province>> GetAllProvinces();
    Task<IEnumerable<District>> GetAllDistrictsByProvinceId(Guid provinceId);
    Task<IEnumerable<Ward>> GetAllWardsByDistrictId(Guid districtId);
}