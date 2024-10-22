using admin_backend.DTOs.DamageClass;
using admin_backend.Interfaces;
using CommonLibrary.DTOs;
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
        private readonly IDamageClassService _damageClassService;

        public DamageClassController(IDamageClassService damageClassService)
        {
            _damageClassService = damageClassService;
        }

        /// <summary>
        /// 取得單筆危害種類
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
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetAll(PagedOperationDto? dto = null)
        {
            return Ok(await _damageClassService.Get(dto: dto));
        }

        /// <summary>
        /// 使用危害類型取得
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetByType(GetDamageClassDto dto)
        {
            return Ok(await _damageClassService.Get(dto));
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
        /// <param name="Id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int Id, UpdateDamageClassDto dto)
        {
            return Ok(await _damageClassService.Update(Id,dto));
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
