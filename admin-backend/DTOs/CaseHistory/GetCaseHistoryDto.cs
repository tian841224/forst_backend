using CommonLibrary.DTOs;

namespace admin_backend.DTOs.CaseHistory
{
    public class GetCaseHistoryDto
    {
        public int CaseId { get; set; }

        /// <summary>
        /// 分頁參數
        /// </summary>
        public PagedOperationDto? Page { get; set; } = new PagedOperationDto();
    }
}
