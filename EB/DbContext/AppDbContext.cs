using EB.Models;
using Microsoft.EntityFrameworkCore;

namespace EB.DbContextData
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<BankHours> BankHours { get; set; }
    }

}
