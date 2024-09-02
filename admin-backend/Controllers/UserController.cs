using admin_backend.DTOs.AdminUser;
using admin_backend.DTOs.User;
using admin_backend.Interfaces;
using admin_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace admin_backend.Controllers
{
    /// <summary>
    /// 會員
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 取得會員
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("{account}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(string account)
        {
            return Ok(await _userService.Get(account));
        }

        /// <summary>
        /// 取得會員
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Get(GetUserDto dto)
        {
            return Ok(await _userService.Get(dto));
        }

        /// <summary>
        /// 新增會員
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Add(AddUserDto dto)
        {
            return Ok(await _userService.Add(dto));
        }

        /// <summary>
        /// 修改會員
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, UpdateUserDto dto)
        {
            return Ok(await _userService.Update(id, dto));
        }

        /// <summary>
        /// 忘記密碼
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            await _userService.ResetPassword(dto);
            return Ok();
        }
    }
}
