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
        [HttpGet]
        [Route("GetFiles")]
        public ActionResult<List<string>> GetNewerFiles(string folderPath, string fileName)
        {
            try
            {
                var newerFiles = _fileService.GetNewerFiles(folderPath, fileName);
                return Ok(newerFiles);

            }catch(FileNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
