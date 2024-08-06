using CommonLibrary.DTOs;

namespace admin_backend.DTOs.DamageType
{
    public class GetDamageClassDto 
    {
        public int TypeId { get; set; }

        /// <summary>
        /// 分頁參數
        /// </summary>
        public PagedOperationDto? Page { get; set; }
    }
}
