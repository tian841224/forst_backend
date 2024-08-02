using admin_backend.DTOs.AdminUser;
using admin_backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace admin_backend.Controllers
{
    /// <summary>
    /// 後台帳號
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class AdminUserController : ControllerBase
    {
        private readonly IAdminUserServices _adminUserServices;

        public AdminUserController(IAdminUserServices adminUserServices)
        {
            _adminUserServices = adminUserServices;
        }

        /// <summary>
        /// 取得後台帳號
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Get(GetAdminUserDto dto)
        {
            return Ok(await _adminUserServices.Get(dto));
        }

        /// <summary>
        /// 取得個人資料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPerson()
        {
            return Ok(await _adminUserServices.Get());
        }

        /// <summary>
        /// 新增後台帳號
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromForm] AddAdminUserDto dto)
        {
            return Ok(await _adminUserServices.Add(dto));
        }

        /// <summary>
        /// 更新後台帳號
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateAdminUserDto dto)
        {
            return Ok(await _adminUserServices.Update(id, dto));
        }
    }
}
