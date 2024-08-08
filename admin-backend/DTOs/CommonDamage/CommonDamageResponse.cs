using admin_backend.Enums;
using CommonLibrary.DTOs;

namespace admin_backend.DTOs.CommonDamage
{
    public class CommonDamageResponse : SortDefaultResponseDto
    {
        /// <summary>
        /// 危害類型ID
        /// </summary>
        public int DamageTypeId { get; set; }

        /// <summary>
        /// 危害類型名稱
        /// </summary>
        public string DamageTypeName { get; set; } = string.Empty;

        /// <summary>
        /// 危害種類ID
        /// </summary>
        public int DamageClassId { get; set; }

        /// <summary>
        /// 危害種類名稱
        /// </summary>
        public string DamageClassName { get; set; } = string.Empty;

        /// <summary>
        /// 病蟲危害名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 危害部位
        /// </summary>
        public List<TreePartEnum> DamagePart { get; set; } = new();

        /// <summary>
        /// 危害特徵
        /// </summary>
        public string DamageFeatures { get; set; } = string.Empty;

        /// <summary>
        /// 防治建議
        /// </summary>
        public string Suggestions { get; set; } = string.Empty;

        /// <summary>
        /// 病蟲封面照片
        /// </summary>
        public List<CommonDamagePhotoResponse> Photo { get; set; } = new();

        /// <summary>
        /// 狀態 0 = 關閉, 1 = 開啟
        /// </summary>
        public StatusEnum Status { get; set; }
    }
}
