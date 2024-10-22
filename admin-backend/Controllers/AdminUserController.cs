using admin_backend.DTOs.AdminUser;
using admin_backend.Interfaces;
using admin_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace admin_backend.Controllers
{
    /// <summary>
    /// ��x�b��
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
        /// ���o��x�b��
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
        /// ���o�ӤH���
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPerson()
        {
            return Ok(await _adminUserServices.Get());
        }

        /// <summary>
        /// �s�W��x�b��
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
        /// ��s��x�b��
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

        /// <summary>
        /// �ѰO�K�X
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            await _adminUserServices.ResetPassword(dto);
            return Ok();
        }
    }
}
