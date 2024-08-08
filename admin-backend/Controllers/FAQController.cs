using admin_backend.DTOs.EpidemicSummary;
using admin_backend.DTOs.FAQ;
using admin_backend.Interfaces;
using admin_backend.Services;
using CommonLibrary.DTOs;
using CommonLibrary.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace admin_backend.Controllers
{
    /// <summary>
    /// 常見問題
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class FAQController : ControllerBase
    {
        private readonly IFAQService fAQService;
        public FAQController(IFAQService fAQService)
        {
            this.fAQService = fAQService;
        }


        /// <summary>
        /// 取得單筆常見問題
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await fAQService.Get(id));
        }

        /// <summary>
        /// 取得全部常見問題
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetAll(PagedOperationDto? dto = null)
        {
            return Ok(await fAQService.Get(dto:dto));
        }

        /// <summary>
        /// 取得常見問題
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Get(GetFAQDto dto)
        {
            return Ok(await fAQService.Get(dto));
        }

        /// <summary>
        /// 新增常見問題
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddFAQDto dto)
        {
            return Ok(await fAQService.Add(dto));
        }

        /// <summary>
        /// 修改常見問題排序
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id,UpdateFAQDto dto)
        {
            return Ok(await fAQService.Update(id,dto));
        }

        /// <summary>
        /// 修改常見問題排序
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateSort(List<SortBasicDto> dto)
        {
            return Ok(await fAQService.UpdateSort(dto));
        }
    }
}
