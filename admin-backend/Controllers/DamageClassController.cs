using admin_backend.Services;
using CommonLibrary.DTOs.DamageClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace admin_backend.Controllers
{
    /// <summary>
    /// 危害種類
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class DamageClassController : ControllerBase
    {
        private readonly DamageClassService _damageClassService;

        public DamageClassController(DamageClassService damageClassService)
        {
            _damageClassService = damageClassService;
        }

        /// <summary>
        /// 取得危害種類
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _damageClassService.Get(id));
        }

        /// <summary>
        /// 取得全部危害種類
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            return Ok(await _damageClassService.Get());
        }

        /// <summary>
        /// 取得全部危害種類
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetByType(int TypeId)
        {
            return Ok(await _damageClassService.Get(TypeId));
        }

        /// <summary>
        /// 新增危害種類
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddDamageClassDto dto)
        {
            return Ok(await _damageClassService.Add(dto));
        }

        /// <summary>
        /// 更新危害種類
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(UpdateDamageClassDto dto)
        {
            return Ok(await _damageClassService.Update(dto));
        }

        /// <summary>
        /// 刪除危害種類
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _damageClassService.Delete(id));
        }
    }
}
