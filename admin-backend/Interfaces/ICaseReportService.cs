using admin_backend.DTOs.CaseReport;

namespace admin_backend.Interfaces
{
    /// <summary>
    /// 案件統計
    /// </summary>
    public interface ICaseReportService
    {
        /// <summary>按照縣市分類</summary>
        Task<List<GroupByCountyResponse>> GroupByCounty(CaseGroupByCountyDto dto);
    }
}
