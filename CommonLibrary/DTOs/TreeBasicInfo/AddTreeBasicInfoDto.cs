using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.DTOs.TreeBasicInfo
{
    public class AddTreeBasicInfoDto
    {
        /// <summary>
        /// 學名
        /// </summary>
        [Required]
        public string ScientificName { get; set; } = string.Empty;

        /// <summary>
        /// 名稱
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
