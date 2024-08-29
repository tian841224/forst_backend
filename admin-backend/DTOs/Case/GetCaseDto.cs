using admin_backend.Enums;
using CommonLibrary.DTOs;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace admin_backend.DTOs.Case
{
    public class GetCaseDto
    {
        ///// <summary>
        ///// 關鍵字
        ///// </summary>
        //public string? Keyword { get; set; } = string.Empty;

        /// <summary>
        /// 案件編號
        /// </summary>
        public int? CaseNumber { get; set; }

        /// <summary>
        /// 開始日期
        /// </summary>
        public string? StartTime { get; set; }

        /// <summary>
        /// 結束日期
        /// </summary>
        public string? EndTime { get; set; }

        /// <summary>
        /// 案件狀態
        /// </summary>
        public CaseStatusEnum? CaseStatus { get; set; }

        /// <summary>
        /// 分頁參數
        /// </summary>
        public PagedOperationDto? Page { get; set; } = new PagedOperationDto();
    }
}
