using Microsoft.AspNetCore.Mvc;
using TestDocker.Service;

namespace TestDocker.Controllers
{
    public class FileWatcherController : Controller
    {
        private readonly FileWatcher _watcher;

        public FileWatcherController(FileWatcher fileWatcher)
        {
            _watcher = fileWatcher;
        }

        [HttpGet("File-watcher")]
        public ActionResult<List<string>> GetNewerFiles()
        {
            try
            {
                var files =  _watcher.GetModifiedFiles();
                return Ok(files);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }            
        }
    }
}
