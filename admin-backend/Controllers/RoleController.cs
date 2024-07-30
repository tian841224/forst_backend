using admin_backend.Services;
using CommonLibrary.DTOs.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace admin_backend.Controllers
{
    /// <summary>
    /// 身分
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class RoleController : ControllerBase
    {
        private readonly RoleServices _roleService;

        public RoleController(RoleServices roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// 取得身分
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get(GetRoleDto dto)
        {
            return Ok(await _roleService.Get(dto));
        }

        /// <summary>
        /// 新增身分
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
        /// 更新身分
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(UpdateRoleDto dto)
        {
            return Ok(await _roleService.Update(dto));
        }

        /// <summary>
        /// 刪除身分
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(DeleteRoleDto dto)
        {
            return Ok(await _roleService.Delete(dto));
        }
    }
}
