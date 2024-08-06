using CommonLibrary.DTOs;

namespace admin_backend.DTOs.Role
{
    public class GetRoleDto 
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        /// <summary>
        /// 分頁參數
        /// </summary>
        public PagedOperationDto? Page { get; set; }
    }
}
