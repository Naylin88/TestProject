using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestProject.Data;
using TestProject.Models;

namespace TestProject.TestProjectAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly TestProjectContext _context;

        public TransactionsController(TestProjectContext context)
        {
            _context = context;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            return await _context.Transactions.ToListAsync();
        }

        // GET: api/Transactions/CurrencyCode
        [HttpGet("{GetByCurrency}")]
        public ActionResult<IEnumerable<Transaction>> GetByCurrency(string currencyCode)
        {
            var result = from tran in _context.Transactions.Where(b => b.CurrencyCode.Contains(currencyCode))
                         select new
                         {
                             id = tran.TransactionId,
                             payment = tran.Amount + " " + tran.CurrencyCode,
                             status = tran.Status
                         };

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [Route("GetByStatus")]
        public ActionResult<IEnumerable<Transaction>> GetByStatus(string status)
        {
            var result = from tran in _context.Transactions.Where(b => b.Status.Contains(status))
                         select new
                         {
                             id = tran.TransactionId,
                             payment = tran.Amount + " " + tran.CurrencyCode,
                             status = tran.Status
                         };

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [Route("GetByDateRange")]
        public ActionResult<IEnumerable<Transaction>> GetByDateRange(string fromDate, string toDate)
        {
            return Ok();
        }
    }
}
