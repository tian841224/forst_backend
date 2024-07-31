using admin_backend.Services;
using CommonLibrary.DTOs.DamageType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace admin_backend.Controllers
{
    /// <summary>
    /// 危害類型
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class DamageTypeController : ControllerBase
    {
        private readonly DamageTypeService _damageTypeService;

        public DamageTypeController(DamageTypeService damageTypeService)
        {
            _damageTypeService = damageTypeService;
        }

        /// <summary>
        /// 取得危害類型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _damageTypeService.Get(id));
        }

        /// <summary>
        /// 取得全部危害類型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            return Ok(await _damageTypeService.Get());
        }

        /// <summary>
        /// 新增危害類型
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddDamageTypeDto dto)
        {
            return Ok(await _damageTypeService.Add(dto));
        }

        /// <summary>
        /// 更新危害類型
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(UpdateDamageTypeDto dto)
        {
            return Ok(await _damageTypeService.Update(dto));
        }

        /// <summary>
        /// 刪除危害類型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _damageTypeService.Delete(id));
        }
    }
}
