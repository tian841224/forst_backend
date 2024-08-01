using admin_backend.Interfaces;
using CommonLibrary.DTOs.TreeBasicInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace admin_backend.Controllers
{
    /// <summary>
    /// 樹木基本資料
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class TreeBasicInfoController : ControllerBase
    {
        private readonly ITreeBasicInfoService _treeBasicInfoService;

        public TreeBasicInfoController(ITreeBasicInfoService treeBasicInfoService)
        {
            _treeBasicInfoService = treeBasicInfoService;
        }

        /// <summary>
        /// 取得全部樹木基本資料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            return Ok(await _treeBasicInfoService.Get());
        }

        /// <summary>
        /// 取得樹木基本資料
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _treeBasicInfoService.Get(id));
        }

        /// <summary>
        /// 新增樹木基本資料
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddTreeBasicInfoDto dto)
        {
            return Ok(await _treeBasicInfoService.Add(dto));
        }

        /// <summary>
        /// 更新樹木基本資料
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, UpdateTreeBasicInfoDto dto)
        {
            return Ok(await _treeBasicInfoService.Update(id, dto));
        }

        /// <summary>
        /// 刪除樹木基本資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _treeBasicInfoService.Delete(id));
        }
    }
}
