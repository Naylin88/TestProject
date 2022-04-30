using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TestProject.Models;

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

        public async Task<List<Transaction>> Upload(IFormFile file) 
        {
            if (file.FileName.EndsWith(".csv"))
            {
                using (var streamReader = new StreamReader(file.OpenReadStream())) 
                {
                    string[] headers = streamReader.ReadLine().Split(',');
                    while (!streamReader.EndOfStream) 
                    {
                        string[] rows = streamReader.ReadLine().Split(',');
                        int TransactionId = int.Parse(rows[0].ToString());
                        double Amount = double.Parse(rows[1].ToString());
                    }
                }
            }
            else { }

            return null;
        }
        public IActionResult Report()
        {
            return View();
        }

      
    }
}
