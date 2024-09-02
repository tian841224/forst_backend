using CommonLibrary.DTOs;

namespace admin_backend.DTOs.CaseDiagnosisResult
{
    public class GetCaseDiagnosisResultDto
    {
        /// <summary>
        /// 案件編號
        /// </summary>
        public int? CaseNumber { get; set; }

        /// <summary>
        /// 常見病蟲害
        /// </summary>
        public int? CommonDamageId { get; set; }

        /// <summary>
        /// 危害類型ID
        /// </summary>
        public int? DamageTypeId { get; set; }

        /// <summary>
        /// 危害種類ID
        /// </summary>
        public int? DamageClassId { get; set; }

        /// <summary>
        /// 分頁參數
        /// </summary>
        public PagedOperationDto? Page { get; set; } = new PagedOperationDto();
    }
}
