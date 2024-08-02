using System.ComponentModel.DataAnnotations;

namespace admin_backend.DTOs.RolePermission
{
    public class UpdateRolePermissionRequestDto
    {
        /// <summary>
        /// 角色管理ID
        /// </summary>
        [Required]
        public int RoleId { get; set; }

        /// <summary>
        /// 權限
        /// </summary>
        public List<Permission> Permissions { get; set; } = new List<Permission>();

        public class Permission
        {
            public int? Id { get; set; }

            /// <summary>
            /// 選單名稱
            /// </summary>
            [Required]
            public string Name { get; set; } = string.Empty;

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
}
