using admin_backend.Interfaces;
using admin_backend.Services;
using CommonLibrary.DTOs.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace admin_backend.Controllers
{
    /// <summary>
    /// 角色管理
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// 取得角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Get(GetRoleDto dto)
        {
            return Ok(await _roleService.Get(dto));
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddRoleDto dto)
        {
            return Ok(await _roleService.Add(dto));
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id ,UpdateRoleDto dto)
        {
            return Ok(await _roleService.Update(id,dto));
        }

        /// <summary>
        /// 刪除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _roleService.Delete(id));
        }
    }
}
