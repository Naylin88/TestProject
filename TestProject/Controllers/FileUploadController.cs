using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TestProject.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly ILogger<FileUploadController> _logger;

        public FileUploadController(ILogger<FileUploadController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Report()
        {
            return View();
        }

      
    }
}
