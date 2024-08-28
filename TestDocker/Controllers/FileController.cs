using Microsoft.AspNetCore.Mvc;
using TestDocker.Service;

namespace TestDocker.Controllers
{
    public class FileController : Controller
    {
        private readonly FileService _fileService;
        public FileController()
        {
            _fileService=new FileService();
        }

        [HttpGet("GetFromOS/{folderPath}/{fileName}")]
        public async Task<ActionResult<List<string>>> GetNewerFiles(string folderPath, string fileName)
        {
            try
            {
                var newerFiles = await _fileService.GetNewerFilesAsync(folderPath, fileName);
                return Ok(newerFiles);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("GetFiles/{folderPath}/{fileName}")]
        public ActionResult<List<string>> GetFiles(string folderPath, string fileName)
        {
            try
            {
                var newerFiles = _fileService.GetFiles(folderPath, fileName);
                return Ok(newerFiles);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


    }
}
