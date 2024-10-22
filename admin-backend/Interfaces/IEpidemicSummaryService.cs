﻿using admin_backend.DTOs.EpidemicSummary;

namespace admin_backend.Interfaces
{
    public interface IEpidemicSummaryService
    {
        Task<EpidemicSummaryResponse> Get();
        Task<EpidemicSummaryResponse> Add(AddEpidemicSummaryDto dto);
    }
}
