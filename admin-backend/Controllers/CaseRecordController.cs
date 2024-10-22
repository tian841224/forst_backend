﻿using admin_backend.DTOs.Case;
using admin_backend.Interfaces;
using admin_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace admin_backend.Controllers
{
    /// <summary>
    /// 案件申請
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class CaseRecordController(ICaseRecordService caseService) : ControllerBase
    {
        private readonly ICaseRecordService _caseService = caseService;

        /// <summary>
        /// 取得單筆案件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _caseService.Get(id));
        }

        /// <summary>
        /// 取得案件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Get(GetCaseRecordDto dto)
        {
            return Ok(await _caseService.Get(dto));
        }

        /// <summary>
        /// 新增案件
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromForm] AddCaseRecordDto dto)
        {
            return Ok(await _caseService.Add(dto));
        }

        /// <summary>
        /// 更新案件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, UpdateCaseRecordDto dto)
        {
            return Ok(await _caseService.Update(id, dto));
        }

        /// <summary>
        /// 上傳檔案
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UploadFile(int id, List<IFormFile> file)
        {
            return Ok(await _caseService.UploadFile(id, file));
        }

        ///// <summary>
        ///// 更新危害類型排序
        ///// </summary>
        ///// <param name="dto"></param>
        ///// <returns></returns>
        //[HttpPut]
        //[Authorize]
        //public async Task<IActionResult> UpdateSort(List<SortBasicDto> dto)
        //{
        //    return Ok(await _damageTypeService.UpdateSort(dto));
        //}

        /// <summary>
        /// 刪除案件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _caseService.Delete(id));
        }

        /// <summary>
        /// 刪除案件檔案
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteFile(int id, int fileId)
        {
            await _caseService.DeleteFile(id, fileId);
            return Ok();
        }
    }
}
