using Microsoft.EntityFrameworkCore;
using TestProject.Models;

namespace TestProject.Data
{
    public class TestProjectContext : DbContext
    {
        public TestProjectContext(DbContextOptions<TestProjectContext> options) : base(options) 
        {

        }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
