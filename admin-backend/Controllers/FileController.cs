using CommonLibrary.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace admin_backend.Controllers
{
    /// <summary>
    /// 檔案相關
    /// </summary>
    [ApiController]
    [Route("[Action]")]
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
            return File(fileBytes, "application/pdf", path);
        }

        [AllowAnonymous]
        [HttpGet("{fileId}")]
        public IActionResult Image(string fileId)
        {
            var path = $"{_fileService.GetFileUploadPath()}/{fileId}";

            // 確認文件存在
            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(path);
            var mimeType = "image/jpeg"; // 默认 MIME 类型
            var extension = Path.GetExtension(path).ToLowerInvariant();
            switch (extension)
            {
                case ".png":
                    mimeType = "image/png";
                    break;
                case ".gif":
                    mimeType = "image/gif";
                    break;
                    // 添加其他文件类型
            }

            return File(fileBytes, mimeType);
        }
    }
}
