using admin_backend.Services;
using CommonLibrary.DTOs.EpidemicSummary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace admin_backend.Controllers
{
    /// <summary>
    /// 疫情簡介
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class EpidemicSummaryController : ControllerBase
    {
        private readonly EpidemicSummaryService _epidemicSummaryService;
        public EpidemicSummaryController(EpidemicSummaryService epidemicSummaryService)
        {
            _epidemicSummaryService = epidemicSummaryService;
        }
        /// <summary>
        /// 取得疫情簡介
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            return Ok(await _epidemicSummaryService.Get());
        }

        /// <summary>
        /// 新增疫情簡介
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddEpidemicSummaryDto dto)
        {
            return Ok(await _epidemicSummaryService.Add(dto));
        }
    }
}
