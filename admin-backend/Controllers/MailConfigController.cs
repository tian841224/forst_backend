using admin_backend.Interfaces;
using CommonLibrary.DTOs.MailConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace admin_backend.Controllers
{
    /// <summary>
    /// 郵寄信件設定
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class MailConfigController : ControllerBase
    {
        private readonly IMailConfigService _mailConfigService;

        public MailConfigController(IMailConfigService mailConfigService)
        {
            _mailConfigService = mailConfigService;
        }

        /// <summary>
        /// 取得郵寄信件設定
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mailConfigService.Get());
        }

        /// <summary>
        /// 新增郵寄信件設定
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddMailConfigDto dto)
        {
            return Ok(await _mailConfigService.Add(dto));
        }

        /// <summary>
        /// 收件測試
        /// </summary>
        /// <param name="email">收信email</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> TestSendEmail(string email)
        {
            await _mailConfigService.TestSendEmail(email);
            return Ok();
        }
    }
}
