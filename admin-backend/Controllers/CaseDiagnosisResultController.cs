using admin_backend.DTOs.CaseDiagnosisResult;
using admin_backend.Interfaces;
using admin_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace admin_backend.Controllers
{
    /// <summary>
    /// 案件回覆
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class CaseDiagnosisResultController(ICaseDiagnosisResultService caseDiagnosisResultService) : ControllerBase
    {
        private readonly ICaseDiagnosisResultService _caseDiagnosisResultService = caseDiagnosisResultService;

        /// <summary>
        /// 取得單筆案件回覆
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _caseDiagnosisResultService.Get(id));
        }

        /// <summary>
        /// 使用CaseID取得單筆案件回覆
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{caseId}")]
        [Authorize]
        public async Task<IActionResult> GetByCaseId(int caseId)
        {
            return Ok(await _caseDiagnosisResultService.GetByCaseId(caseId));
        }

        /// <summary>
        /// 取得案件回覆
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Get(GetCaseDiagnosisResultDto dto)
        {
            return Ok(await _caseDiagnosisResultService.Get(dto));
        }

        /// <summary>
        /// 新增案件回覆
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddCaseDiagnosisResultDto dto)
        {
            return Ok(await _caseDiagnosisResultService.Add(dto));
        }

        /// <summary>
        /// 更新案件回覆
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, UpdateCaseDiagnosisResultDto dto)
        {
            return Ok(await _caseDiagnosisResultService.Update(id, dto));
        }

        /// <summary>
        /// 上傳檔案
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UploadPhoto(List<IFormFile> photo)
        {
            return Ok(await _caseDiagnosisResultService.UploadPhoto(photo));
        }

        /// <summary>
        /// 刪除案件回覆
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _caseDiagnosisResultService.Delete(id));
        }
    }
}
