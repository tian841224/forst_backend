using admin_backend.DTOs.CommonDamage;
using admin_backend.DTOs.DamageType;
using admin_backend.Interfaces;
using CommonLibrary.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace admin_backend.Controllers
{
    /// <summary>
    /// 常見病蟲害
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class CommonDamageController : ControllerBase
    {
        private readonly ICommonDamageService _commonDamageService;

        public CommonDamageController(ICommonDamageService commonDamageService)
        {
            _commonDamageService = commonDamageService;
        }


        /// <summary>
        /// 取得常見病蟲害
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _commonDamageService.Get(id));
        }

        /// <summary>
        /// 取得全部常見病蟲害
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(PagedOperationDto? dto = null)
        {
            return Ok(await _commonDamageService.Get(dto: dto));
        }

        /// <summary>
        /// 取得常見病蟲害
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Get(GetCommonDamageDto dto)
        {
            return Ok(await _commonDamageService.Get(dto));
        }

        /// <summary>
        /// 新增常見病蟲害
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddCommonDamageDto dto)
        {
            return Ok(await _commonDamageService.Add(dto));
        }

        /// <summary>
        /// 修改常見病蟲害
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, UpdateCommonDamageDto dto)
        {
            return Ok(await _commonDamageService.Update(id, dto));
        }

        /// <summary>
        /// 上傳常見病蟲害圖片
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UploadFile(int Id, [FromForm] CommonDamagePhotoDto dto)
        {
            return Ok(await _commonDamageService.UploadFile(Id, dto));
        }

        /// <summary>
        /// 修改常見病蟲害排序
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateSort(List<SortBasicDto> dto)
        {
            return Ok(await _commonDamageService.UpdateSort(dto));
        }

        /// <summary>
        /// 修改常見病蟲害圖片排序
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateFileSort(int id, List<UpdateFileSortDto> dto)
        {
            return Ok(await _commonDamageService.UpdateFileSort(id,dto));
        }

        /// <summary>
        /// 刪除常見病蟲害
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _commonDamageService.Delete(id));
        }

        /// <summary>
        /// 刪除圖片
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteFile(int id, string fileId)
        {
            await _commonDamageService.DeleteFile(id, fileId);
            return Ok();
        }
    }
}
