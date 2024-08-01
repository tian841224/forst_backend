using admin_backend.Interfaces;
using admin_backend.Services;
using CommonLibrary.DTOs.Common;
using CommonLibrary.DTOs.ForestCompartmentLocation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace admin_backend.Controllers
{
    /// <summary>
    /// 林班位置
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class ForestCompartmentLocationController : Controller
    {
        private readonly IForestCompartmentLocationService _forestCompartmentLocationService;

        public ForestCompartmentLocationController(IForestCompartmentLocationService forestCompartmentLocationService)
        {
            _forestCompartmentLocationService = forestCompartmentLocationService;
        }


        /// <summary>
        /// 取得單筆林班位置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _forestCompartmentLocationService.Get(id));
        }

        /// <summary>
        /// 取得全部林班位置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            return Ok(await _forestCompartmentLocationService.Get());
        }

        /// <summary>
        /// 取得林班位置
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Get(GetForestCompartmentLocationDto dto)
        {
            return Ok(await _forestCompartmentLocationService.Get(dto));
        }

        /// <summary>
        /// 新增林班位置
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddForestCompartmentLocationDto dto)
        {
            return Ok(await _forestCompartmentLocationService.Add(dto));
        }

        /// <summary>
        /// 更新林班位置
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, UpdateForestCompartmentLocationDto dto)
        {
            return Ok(await _forestCompartmentLocationService.Update(id, dto));
        }

        /// <summary>
        /// 更新林班位置排序
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateSort(List<SortBasicDto> dto)
        {
            return Ok(await _forestCompartmentLocationService.UpdateSort(dto));
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
            return Ok(await _forestCompartmentLocationService.Delete(id));
        }
    }
}
