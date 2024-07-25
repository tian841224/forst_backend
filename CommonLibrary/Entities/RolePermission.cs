using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonLibrary.Entities
{
    public class RolePermission : DefaultEntity
    {
        /// <summary>
        /// 選單名稱
        /// </summary>
        [Required]
        [Comment("選單名稱")]
        [StringLength(50)] 
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 角色管理ID
        /// </summary>
        [Required]
        [Comment("角色管理ID")]
        public int RoleId { get; set; }

        ///// <summary>
        ///// 權限管理
        ///// </summary>
        //[Required]
        //[Comment("權限管理")]
        //public int PermissionsId { get; set; }

        /// <summary>
        /// 檢視
        /// </summary>
        [Required]
        [Comment("檢視")]
        [DefaultValue(false)]
        public bool View {  get; set; }

        /// <summary>
        /// 新增
        /// </summary>
        [Required]
        [Comment("新增")]
        [DefaultValue(false)]
        public bool Add { get; set; }

        /// <summary>
        /// 指派
        /// </summary>
        [Required]
        [Comment("指派")]
        [DefaultValue(false)]
        public bool Sign { get; set; }

        /// <summary>
        /// 編輯
        /// </summary>
        [Required]
        [Comment("編輯")]
        [DefaultValue(false)]
        public bool Edit { get; set; }

        /// <summary>
        /// 刪除
        /// </summary>
        [Required]
        [Comment("刪除")]
        [DefaultValue(false)]
        public bool Delete { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; } 

        //[ForeignKey("PermissionsId")]
        //public virtual Permission Permission { get; set; }
    }
}
