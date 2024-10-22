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

        /// <summary>按照危害案件種類分類</summary>
        Task<List<GroupByDamageClassResponse>> GroupByDamageClass(CaseGroupByDamageClassDto dto);
        //Task<List<GroupByDamageTypeResponse>> GroupByDamageType(CaseGroupByDamageTypeDto dto);

        /// <summary>按照危害種類地理位置分類</summary>
        Task<List<GroupByDamageLocationResponse>> GroupByDamageLocation(CaseGroupByDamageLocationDto dto);

        /// <summary>按照常見病蟲害地理位置分類</summary>
        Task<List<GroupByCountyDamageResponse>> GroupByCountyDamage(CaseGroupByCountyDamageDto dto);

        /// <summary>按照年月份分類</summary>
        Task<List<GroupByMonthResponse>> GroupByMonth(CaseGroupByMonthDto dto);

        /// <summary>按照常見病蟲害分類</summary>
        Task<List<GroupByCommonamageResponse>> GroupByCommonamage(CaseGroupByCommonamageDto dto);
    }
}
