using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace TenderAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class FileManagerController : ControllerBase
    {
        private IWebHostEnvironment _hostingEnvironment;

        public FileManagerController(IWebHostEnvironment environment)
        {
            _hostingEnvironment = environment;
        }

        [HttpPost]
        [Route("upload/{practiceId}")]
        public async Task<IActionResult> UploadFile(IFormFile file, int practiceId, [FromQuery] string folderPath)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("File non valido.");
                }

                var destinationPath = Path.Combine(folderPath, practiceId.ToString());

                if (!Directory.Exists(destinationPath))
                {
                    Directory.CreateDirectory(destinationPath);
                }

                // 36 caratteri
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(destinationPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // salvare percorso nel db

                return Ok(Path.Combine(destinationPath, fileName));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Errore durante il caricamento del file: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("download")]
        public IActionResult DownloadFile([FromQuery] string file, [FromQuery] string folderPath)
        {
            try
            {
                var filePath = Path.Combine(folderPath, file);

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound();
                }

                var contentType = GetFileContentType(file);

                return PhysicalFile(filePath, contentType, file);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Errore durante il caricamento del file: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("files")]
        public IActionResult GetAllFiles()
        {
            var result = new List<string>();

            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            if (Directory.Exists(uploads))
            {
                var provider = _hostingEnvironment.ContentRootFileProvider;
                foreach (string fileName in Directory.GetFiles(uploads))
                {
                    var fileInfo = provider.GetFileInfo(fileName);
                    result.Add(fileInfo.Name);
                }
            }
            return Ok(result);
        }

        private string GetFileContentType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }
    }
}
