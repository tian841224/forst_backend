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
        /// 取得案件縣市分類
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GroupByCounty(CaseGroupByCountyDto dto)
        {
            return Ok(await _caseReportService.GroupByCounty(dto));
        }
    }
}
