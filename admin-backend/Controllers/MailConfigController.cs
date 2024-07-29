using admin_backend.Services;
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
        private readonly MailConfigService _mailConfigService;

        public MailConfigController(MailConfigService mailConfigService)
        {
            _mailConfigService = mailConfigService;
        }

        /// <summary>
        /// 取得郵寄信件設定
        /// </summary>
        /// <returns></returns>
        [HttpPost]
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
    }
}
