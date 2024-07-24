using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.Entities
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public class Role : DefaultEntity
    {
        /// <summary>
        /// 角色名稱
        /// </summary>
        [Comment("角色名稱")]
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
