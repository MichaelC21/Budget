using BudgetApp.Data.Entites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Data
{
    public class BudgetDbContext(DbContextOptions<BudgetDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transaction>()
                .HasIndex(t => t.UserID);
        }
    }
}
