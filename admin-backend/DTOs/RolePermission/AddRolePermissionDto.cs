using System.ComponentModel.DataAnnotations;

namespace admin_backend.DTOs.RolePermission
{
    public class AddRolePermissionDto
    {
        /// <summary>
        /// 選單名稱
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 角色管理ID
        /// </summary>
        [Required]
        public int RoleId { get; set; }

        /// <summary>
        /// 檢視
        /// </summary>
        [Required]
        public bool View { get; set; }

        /// <summary>
        /// 新增
        /// </summary>
        [Required]
        public bool Add { get; set; }

        /// <summary>
        /// 指派
        /// </summary>
        [Required]
        public bool Sign { get; set; }

        /// <summary>
        /// 編輯
        /// </summary>
        [Required]
        public bool Edit { get; set; }

        /// <summary>
        /// 刪除
        /// </summary>
        [Required]
        public bool Delete { get; set; }
    }
}
