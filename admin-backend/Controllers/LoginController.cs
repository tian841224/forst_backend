using admin_backend.DTOs.Login;
using admin_backend.Interfaces;
using admin_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace admin_backend.Controllers
{
    /// <summary>
    /// µn¤J
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]

    public class LoginController : ControllerBase
    {
        private readonly ILoginServices _loginServices;

        public LoginController(ILoginServices loginServices)
        {
            _loginServices = loginServices;
        }

        /// <summary>
        /// µn¤J
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            return Ok(await _loginServices.Login(dto));
        }

        /// <summary>
        /// §Ñ°O±K½X
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            await _loginServices.ResetPassword(dto);
            return Ok();
        }
    }
}