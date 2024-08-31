using admin_backend.DTOs.AdSetting;
using admin_backend.Interfaces;
using CommonLibrary.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace admin_backend.Controllers
{
    /// <summary>
    /// 官網廣告版位
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class AdSettingController : ControllerBase
    {
        private readonly IAdSettingService _adSettingService;

        public AdSettingController(IAdSettingService adSettingService)
        {
            _adSettingService = adSettingService;
        }

        /// <summary>
        /// 取得單筆官網廣告版位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _adSettingService.Get(id));
        }

        /// <summary>
        /// 取得全部官網廣告版位
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(PagedOperationDto? dto = null)
        {
            return Ok(await _adSettingService.Get(dto: dto));
        }

        /// <summary>
        /// 取得官網廣告版位
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Get(GetdSettingDto dto)
        {
            return Ok(await _adSettingService.Get(dto));
        }

        /// <summary>
        /// 新增官網廣告版位
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromForm]AddAdSettingDto dto)
        {
            return Ok(await _adSettingService.Add(dto));
        }

        /// <summary>
        /// 更新官網廣告版位
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromForm]UpdateAdSettingDto dto)
        {
            return Ok(await _adSettingService.Update(id, dto));
        }

        /// <summary>
        /// 上傳官網廣告版位圖片
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UploadFile(int id, [FromForm] AdSettingPhotoDto dto)
        {
            await _adSettingService.UploadFile(id, dto);
            return Ok();
        }

        /// <summary>
        /// 刪除官網廣告版位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _adSettingService.Delete(id));
        }
    }
}
