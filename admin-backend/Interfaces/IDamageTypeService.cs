﻿using admin_backend.DTOs.DamageType;
using CommonLibrary.DTOs;

namespace admin_backend.Interfaces
{
    public interface IDamageTypeService
    {
        Task<List<DamageTypeResponse>> Get(int? Id = null, PagedOperationDto? dto = null);
        Task<List<DamageTypeResponse>> Get(GetDamageTypeDto dto);
        Task<DamageTypeResponse> Add(AddDamageTypeDto dto);
        Task<DamageTypeResponse> Update(UpdateDamageTypeDto dto);
        Task<List<DamageTypeResponse>> UpdateSort(List<SortBasicDto> dto);
        Task<DamageTypeResponse> Delete(int Id);
    }
}
