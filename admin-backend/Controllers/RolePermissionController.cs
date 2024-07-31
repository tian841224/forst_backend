using admin_backend.Services;
using CommonLibrary.DTOs.RolePermission;
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
        private readonly RolePermissionService _rolePermissionService;
        public RolePermissionController(RolePermissionService rolePermissionService)
        {
            _rolePermissionService = rolePermissionService;
        }

        /// <summary>
        /// 取得身分權限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _rolePermissionService.Get(id));
        }

        /// <summary>
        /// 新增身分權限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(List<AddRolePermissionDto> dto)
        {
            return Ok(await _rolePermissionService.Add(dto));
        }

        /// <summary>
        /// 修改身分權限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(UpdateRolePermissionDto dto)
        {
            return Ok(await _rolePermissionService.Update(dto));
        }

    }
}
