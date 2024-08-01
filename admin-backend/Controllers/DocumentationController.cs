﻿using admin_backend.Interfaces;
using CommonLibrary.DTOs.Documentation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace admin_backend.Controllers
{
    /// <summary>
    /// 會員註冊使用條款
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class DocumentationController : ControllerBase
    {
        private readonly IDocumentationService _documentationService;
        public DocumentationController(IDocumentationService documentationService)
        {
            _documentationService = documentationService;
        }

        /// <summary>
        /// 取得會員註冊使用條款
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            return Ok(await _documentationService.Get());
        }

        /// <summary>
        /// 新增會員註冊使用條款
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddDocumentationDto dto)
        {
            return Ok(await _documentationService.Add(dto));
        }
    }
}
