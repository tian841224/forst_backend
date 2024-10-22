using admin_backend.DTOs.AdminUser;
using admin_backend.DTOs.Login;
using admin_backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace admin_backend.Controllers
{
    /// <summary>
    /// �n�J
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
        /// �n�J��x
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginBackEnd(LoginDto dto)
        {
            return Ok(await _loginServices.LoginBackEnd(dto));
        }

        /// <summary>
        /// �n�J�e�x
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginFrontEnd(LoginFrontEndDto dto)
        {
            return Ok(await _loginServices.LoginFrontEnd(dto));
        }
    }
}