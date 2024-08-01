﻿using CommonLibrary.DTOs.DamageType;

namespace admin_backend.Interfaces
{
    public interface IDamageTypeService
    {
        Task<List<DamageTypeResponse>> Get(int? Id = null);
        Task<DamageTypeResponse> Add(AddDamageTypeDto dto);
        Task<DamageTypeResponse> Update(UpdateDamageTypeDto dto);
        Task<DamageTypeResponse> Delete(int Id);
    }
}
