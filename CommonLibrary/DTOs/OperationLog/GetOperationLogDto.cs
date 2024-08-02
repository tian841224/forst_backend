﻿using CommonLibrary.DTOs.Common;
using CommonLibrary.Enums;

namespace CommonLibrary.DTOs.OperationLog
{
    public class GetOperationLogDto: PagedOperationDto
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public int? RoleId { get; set; } 

        /// <summary>
        /// 帳號ID
        /// </summary>
        public int? AdminUserId { get; set; }

        /// <summary>
        /// 異動類型 新增 = 1, 指派 = 2, 編輯 = 3, 刪除 = 4
        /// </summary>
        public ChangeTypeEnum? Type { get; set; }

        /// <summary>
        /// 起始時間
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}
