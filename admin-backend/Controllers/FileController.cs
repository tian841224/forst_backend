using CommonLibrary.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace admin_backend.Controllers
{
    /// <summary>
    /// 檔案相關
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [AllowAnonymous]
        [HttpGet("{fileId}")]
        public  IActionResult File(string fileId)
        {
            var path = $"{_fileService.GetFileUploadPath()}/{fileId}";

            // 確認文件存在
            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, "application/octet-stream", path);
        }
    }
}
