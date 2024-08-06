using admin_backend.DTOs.ForestDiseasePublications;
using admin_backend.Interfaces;
using CommonLibrary.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace admin_backend.Controllers
{
    /// <summary>
    /// 林木疫情出版品
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class ForestDiseasePublicationsController : ControllerBase
    {
        private readonly IForestDiseasePublicationsService _forestDiseasePublicationsService;
        public ForestDiseasePublicationsController(IForestDiseasePublicationsService forestDiseasePublicationsService)
        {
            _forestDiseasePublicationsService = forestDiseasePublicationsService;
        }

        /// <summary>
        /// 取得單筆林木疫情出版品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _forestDiseasePublicationsService.Get(id));
        }

        /// <summary>
        /// 取得全部林木疫情出版品
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetAll(PagedOperationDto? dto = null)
        {
            return Ok(await _forestDiseasePublicationsService.Get(dto: dto));
        }

        /// <summary>
        /// 取得林木疫情出版品
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Get(GetForestDiseasePublicationsDto dto)
        {
            return Ok(await _forestDiseasePublicationsService.Get(dto));
        }

        /// <summary>
        /// 新增林木疫情出版品
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromForm] AddForestDiseasePublicationsDto dto)
        {
            return Ok(await _forestDiseasePublicationsService.Add(dto));
        }

        /// <summary>
        /// 更新林木疫情出版品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, UpdateForestDiseasePublicationsDto dto)
        {
            return Ok(await _forestDiseasePublicationsService.Update(id, dto));
        }

        /// <summary>
        /// 修改上傳檔案
        /// </summary>
        /// <param name="id"></param>
        /// <param name="file">檔案</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateFile(int id, List<IFormFile> file)
        {
            await _forestDiseasePublicationsService.UpdateFile(id, file);
            return Ok();
        }

        /// <summary>
        /// 更新林木疫情出版品排序
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateSort(List<SortBasicDto> dto)
        {
            return Ok(await _forestDiseasePublicationsService.UpdateSort(dto));
        }

        /// <summary>
        /// 刪除林木疫情出版品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _forestDiseasePublicationsService.Delete(id));
        }

        /// <summary>
        /// 刪除附件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fileId">檔案ID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteFile(int id,string fileId)
        {
            await _forestDiseasePublicationsService.DeleteFile(id,fileId);
            return Ok();
        }
    }
}
