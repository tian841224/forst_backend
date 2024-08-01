using CommonLibrary.Enums;
using Microsoft.AspNetCore.Http;

namespace CommonLibrary.DTOs.AdminUser
{
    public class UpdateAdminUserDto
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string? Name { get; set; }

        ///// <summary>
        ///// 帳號
        ///// </summary>
        //public string? Account { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        public string? pKey { get; set; }

        /// <summary>
        /// 信箱
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// 照片
        /// </summary>
        public IFormFile? Photo { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public int? RoleId { get; set; }

        /// <summary>
        /// 狀態 0 = 關閉, 1 = 開啟 
        /// </summary>
        public StatusEnum? Status { get; set; }
    }
}
