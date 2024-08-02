using admin_backend.DTOs.OperationLog;
using admin_backend.Interfaces;
using admin_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace admin_backend.Controllers
{
    /// <summary>
    /// 系統操作紀錄
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class OperationLogController : ControllerBase
    {

        private readonly IOperationLogService _operationLogService;

        public OperationLogController(IOperationLogService operationLogService)
        {
            _operationLogService = operationLogService;
        }

        /// <summary>
        /// 取得系統操作紀錄
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Get(GetOperationLogDto dto)
        {
            return Ok(await _operationLogService.Get(dto));
        }
    }
}
