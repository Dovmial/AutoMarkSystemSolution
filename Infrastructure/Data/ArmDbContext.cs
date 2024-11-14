
using Domain.Aggregates.Lines;
using Domain.Aggregates.MarkingCodeHistory;
using Domain.Aggregates.MarkingCodes;
using Domain.Aggregates.Products;
using Domain.Aggregates.Sessions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ArmDbContext(DbContextOptions options): DbContext(options)
    {
        public DbSet<MarkingCodeEntity> MarkingCodes { get; set; }
        public DbSet<SessionEntity> Sessions { get; set; }
        public DbSet<MarkingCodeHistoryEntity> MarkingCodesHistories { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductionLineEntity> ProductionLines { get; set; }
        
       // public DbSet<StatisticEntity> Statistics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ArmDbContext).Assembly);
        }
    }
}
