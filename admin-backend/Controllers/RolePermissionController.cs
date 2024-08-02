using admin_backend.DTOs.RolePermission;
using admin_backend.Interfaces;
using admin_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace admin_backend.Controllers
{
    /// <summary>
    /// 身分權限
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class RolePermissionController : Controller
    {
        private readonly IRolePermissionService _rolePermissionService;
        public RolePermissionController(IRolePermissionService rolePermissionService)
        {
            _rolePermissionService = rolePermissionService;
        }

        /// <summary>
        /// 取得角色權限
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _rolePermissionService.Get(id));
        }

        ///// <summary>
        ///// 新增角色權限
        ///// </summary>
        ///// <param name="dto"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Authorize]
        //public async Task<IActionResult> Add(AddRolePermissionRequestDto dto)
        //{
        //    return Ok(await _rolePermissionService.Add(dto));
        //}

        /// <summary>
        /// 修改角色權限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(UpdateRolePermissionRequestDto dto)
        {
            return Ok(await _rolePermissionService.Update(dto));
        }

    }
}
