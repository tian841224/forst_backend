using System.ComponentModel.DataAnnotations;
using CommonLibrary.DTOs.Common;

namespace CommonLibrary.DTOs.TreeBasicInfo
{
    public class AddTreeBasicInfoDto:SortDto
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
