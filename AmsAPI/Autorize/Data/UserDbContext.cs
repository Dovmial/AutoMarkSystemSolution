using AmsAPI.Autorize.Models;
using Microsoft.EntityFrameworkCore;

namespace AmsAPI.Autorize.Data
{
    public class UserDbContext(DbContextOptions options): DbContext(options)
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);
        }

    }
}
