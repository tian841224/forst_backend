using admin_backend.Services;
using CommonLibrary.DTOs.Login;
using Microsoft.AspNetCore.Mvc;

namespace admin_backend.Controllers
{
        [ApiController]
    [Route("[controller]/[action]")]
    /// <summary>
    /// 登入
    /// </summary>
    public class LoginController : ControllerBase
    {
        private readonly LoginServices _loginServices;

        public LoginController(LoginServices loginServices)
        {
            _loginServices = loginServices;
        }

        /// <summary>
        /// 取得驗證碼
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCaptcha()
        {
            return Ok(await _loginServices.GetCaptchaAsync());
        }


        /// <summary>
        /// 登入
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            return Ok(await _loginServices.Login(dto));
        }
    }
}