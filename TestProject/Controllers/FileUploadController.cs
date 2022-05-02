using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
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
            try
            {
                if (file.FileName.EndsWith(".csv"))
                {

                    using (var streamReader = new StreamReader(file.OpenReadStream()))
                    {
                        while (!streamReader.EndOfStream) transactionList.Add(streamReader.ReadLine());
                    }
                   
                    foreach (var transaction in transactionList)
                    {
                        if (string.IsNullOrEmpty(transaction.Split(',')[0])) return BadRequest("Invalid Field");
                        if (string.IsNullOrEmpty(transaction.Split(',')[1])) return BadRequest("Invalid Field");
                        if (string.IsNullOrEmpty(transaction.Split(',')[2])) return BadRequest("Invalid Field");
                        if (string.IsNullOrEmpty(transaction.Split(',')[3])) return BadRequest("Invalid Field");
                        if (string.IsNullOrEmpty(transaction.Split(',')[4])) return BadRequest("Invalid Field");

                        transactions.Add(new Transaction
                        {
                            Id = Guid.NewGuid(),
                            TransactionId = transaction.Split(',')[0],
                            Amount = Convert.ToDecimal(transaction.Split(',')[1]),
                            CurrencyCode = transaction.Split(',')[2],
                            TransactionDate = DateTime.ParseExact((transaction.Split(',')[3]), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                            Status = transaction.Split(',')[4]

                        });
                    }

                    _context.AddRange(transactions);
                    await _context.SaveChangesAsync();

                    return Ok(200);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            if (file.FileName.EndsWith(".xml"))
            {
                try
                {
                    var doc = new XmlDocument();
                    using (var streamReader = new StreamReader(file.OpenReadStream()))
                    {
                        var contents = streamReader.ReadToEnd();
                        doc.LoadXml(contents);

                        XmlNodeList nodes = doc.SelectNodes("/Transactions/Transaction");
                        foreach (XmlNode node in nodes)
                        {
                            string transactionId = node.Attributes?.GetNamedItem("id")?.InnerText; if (string.IsNullOrEmpty(transactionId)) return BadRequest("Invalid Field");
                            string date = node.ChildNodes[0]?.InnerText; if (string.IsNullOrEmpty(date)) return BadRequest("Invalid Field");
                            string amount = node.ChildNodes[1].FirstChild?.InnerText; if (string.IsNullOrEmpty(amount)) return BadRequest("Invalid Field");
                            string currencyCode = node.ChildNodes[1].LastChild?.InnerText; if (string.IsNullOrEmpty(currencyCode)) return BadRequest("Invalid Field");
                            string status = node.ChildNodes[2]?.InnerText; if (string.IsNullOrEmpty(status)) return BadRequest("Invalid Field");

                            transactions.Add(new Transaction
                            {
                                Id = Guid.NewGuid(),
                                TransactionId = transactionId.ToString(),
                                Amount = Convert.ToDecimal(amount),
                                CurrencyCode = currencyCode,
                                TransactionDate = DateTime.ParseExact(date, "s", null),
                                Status = status

                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                }
                _context.AddRange(transactions);
                await _context.SaveChangesAsync();
                return Ok(200);
            }
       
            else { return BadRequest("Invalid File Extension!"); }
        }
        public IActionResult Report()
        {
            return View();
        }     
    }
}
