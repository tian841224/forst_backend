using admin_backend.DTOs.News;
using admin_backend.Interfaces;
using CommonLibrary.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace admin_backend.Controllers
{
    /// <summary>
    /// 最新消息
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        /// <summary>
        /// 取得單筆最新消息
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _newsService.Get(id));
        }

        /// <summary>
        /// 取得全部最新消息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(PagedOperationDto? dto = null)
        {
            return Ok(await _newsService.Get(dto: dto));
        }

        /// <summary>
        /// 取得最新消息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Get(GetNewsDto dto)
        {
            return Ok(await _newsService.Get(dto));
        }

        /// <summary>
        /// 新增最新消息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddNewsDto dto)
        {
            return Ok(await _newsService.Add(dto));
        }

        /// <summary>
        /// 更新最新消息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, UpdateNewsDto dto)
        {
            return Ok(await _newsService.Update(id, dto));
        }

        /// <summary>
        /// 刪除最新消息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _newsService.Delete(id));
        }
    }
}
