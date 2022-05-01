using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TestProject.Data;
using TestProject.Models;

namespace TestProject.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly ILogger<FileUploadController> _logger;
        private readonly TestProjectContext _context;

        public FileUploadController(ILogger<FileUploadController> logger, TestProjectContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upload(IFormFile file) 
        {
            var transactions = new List<Transaction>();
            var transactionList = new List<string>();
           
            if (file.FileName.EndsWith(".csv"))
            {
                using (var streamReader = new StreamReader(file.OpenReadStream()))
                {
                    while (!streamReader.EndOfStream) transactionList.Add(streamReader.ReadLine());                    
                }
                foreach (var transaction in transactionList)
                {
                    transactions.Add(new Transaction
                    {
                        Id = Guid.NewGuid(),
                        TransactionId = transaction.Split(',')[0],
                        Amount = Convert.ToDecimal(transaction.Split(',')[1]),
                        CurrencyCode = transaction.Split(',')[2],
                        TransactionDate = DateTime.ParseExact((transaction.Split(',')[3]),"dd/MM/yyyy HH:mm:ss",CultureInfo.InvariantCulture),
                        Status = transaction.Split(',')[4]
                        
                    }) ;
                }
                
                _context.AddRange(transactions); 
                await _context.SaveChangesAsync();
                
                return Ok(200);
            }
            if (file.FileName.EndsWith(".xml")) 
            {
                return Ok("good xml");
            }
            else { return BadRequest("Invalid File Extension!"); }

        }
        public IActionResult Report()
        {
            return View();
        }
      
    }
}
