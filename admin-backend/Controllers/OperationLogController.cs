using admin_backend.Services;
using CommonLibrary.DTOs.OperationLog;
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

        private readonly OperationLogService _operationLogService;

        public OperationLogController(OperationLogService operationLogService)
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
