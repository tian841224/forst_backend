using admin_backend.Enums;
using CommonLibrary.DTOs;

namespace admin_backend.DTOs.DamageClass
{
    public class DamageClassResponse : DefaultResponseDto
    {
        /// <summary>
        /// 危害種類
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 危害類型ID
        /// </summary>
        public int DamageTypeId { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public StatusEnum Status { get; set; }
    }
}
