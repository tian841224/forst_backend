using admin_backend.DTOs.CaseReport;
using admin_backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace admin_backend.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CaseReportController(ICaseReportService caseReportService) : ControllerBase
    {
        private readonly ICaseReportService _caseReportService = caseReportService;

        /// <summary>
        /// 行政案件查詢
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GroupByCounty(CaseGroupByCountyDto dto)
        {
            return Ok(await _caseReportService.GroupByCounty(dto));
        }

        /// <summary>
        /// 危害案件種類查詢
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GroupByDamageClass(CaseGroupByDamageClassDto dto)
        {
            return Ok(await _caseReportService.GroupByDamageClass(dto));
        }

        /// <summary>
        /// 危害種類地理位置查詢
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GroupByDamageLocation(CaseGroupByDamageLocationDto dto)
        {
            return Ok(await _caseReportService.GroupByDamageLocation(dto));
        }

        /// <summary>
        /// 危害地理位置查詢
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GroupByCountyDamage(CaseGroupByCountyDamageDto dto)
        {
            return Ok(await _caseReportService.GroupByCountyDamage(dto));
        }

        /// <summary>
        /// 年月案件統計查詢
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GroupByMonth(CaseGroupByMonthDto dto)
        {
            return Ok(await _caseReportService.GroupByMonth(dto));
        }

        /// <summary>
        /// 危害統計查詢
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GroupByCommonamage(CaseGroupByCommonamageDto dto)
        {
            return Ok(await _caseReportService.GroupByCommonamage(dto));
        }
    }
}
