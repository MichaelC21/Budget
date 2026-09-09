using BudgetApp.Data.Entites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Data
{
    public class BudgetDbContext(DbContextOptions<BudgetDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<Transaction> Transactions{ get; set; }
    }
}
