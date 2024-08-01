using admin_backend.Interfaces;
using admin_backend.Services;
using CommonLibrary.DTOs.Login;
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
    }
}