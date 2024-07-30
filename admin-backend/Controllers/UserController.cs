using admin_backend.Services;
using CommonLibrary.DTOs.User;
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
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
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
        [Authorize]
        public async Task<IActionResult> Add(AddUserDto dto)
        {
            return Ok(await _userService.Add(dto));
        }

        /// <summary>
        /// 修改會員
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(UpdateUserDto dto)
        {
            return Ok(await _userService.Update(dto));
        }

    }
}
