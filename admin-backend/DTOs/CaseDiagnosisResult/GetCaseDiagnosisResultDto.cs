using CommonLibrary.DTOs;

namespace admin_backend.DTOs.CaseDiagnosisResult
{
    public class GetCaseDiagnosisResultDto
    {
        /// <summary>
        /// 案件編號
        /// </summary>
        public int? CaseId { get; set; }

        /// <summary>
        /// 常見病蟲害
        /// </summary>
        public int? CommonDamageId { get; set; }

        /// <summary>
        /// 分頁參數
        /// </summary>
        public PagedOperationDto? Page { get; set; } = new PagedOperationDto();
    }
}
