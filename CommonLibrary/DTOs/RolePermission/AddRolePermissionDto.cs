﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CommonLibrary.DTOs.RolePermission
{
    public class AddRolePermissionDto
    {
        /// <summary>
        /// 選單名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 角色管理ID
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 檢視
        /// </summary>
        public bool View { get; set; }

        /// <summary>
        /// 新增
        /// </summary>
        public bool Add { get; set; }

        /// <summary>
        /// 指派
        /// </summary>
        public bool Sign { get; set; }

        /// <summary>
        /// 編輯
        /// </summary>
        public bool Edit { get; set; }

        /// <summary>
        /// 刪除
        /// </summary>
        public bool Delete { get; set; }
    }
}
