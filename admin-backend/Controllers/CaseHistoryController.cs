using admin_backend.DTOs.Case;
using admin_backend.DTOs.CaseHistory;
using admin_backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace admin_backend.Controllers
{    
    /// <summary>
     /// 案件歷程
     /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class CaseHistoryController : ControllerBase
    {
        private readonly ICaseHistoryService _caseHistoryService;

        public CaseHistoryController(ICaseHistoryService caseHistoryService)
        {
            _caseHistoryService = caseHistoryService;
        }

        /// <summary>
        /// 取得單筆案件歷程
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _caseHistoryService.Get(id));
        }

        /// <summary>
        /// 取得案件歷程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Get(GetCaseHistoryDto dto)
        {
            return Ok(await _caseHistoryService.Get(dto));
        }
    }
}
