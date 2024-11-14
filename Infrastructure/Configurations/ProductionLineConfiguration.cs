

using Domain.Aggregates.Lines;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal class ProductionLineConfiguration : IEntityTypeConfiguration<ProductionLineEntity>
    {
        public void Configure(EntityTypeBuilder<ProductionLineEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasConversion(l => l.Value, value => new ProductionLineId(value))
                .ValueGeneratedOnAdd();

            builder.HasIndex(x => x.Name)
                .IsUnique();
            builder.Property(x => x.Name)
                .IsRequired();
                
                

            builder.HasMany(l => l.Products)
                .WithMany(p => p.ProductionLines)
                .UsingEntity(t => t.ToTable("ProductionLinesProducts"));

            builder.HasMany(pl => pl.Sessions)
                .WithOne(s => s.ProductionLine)
                .HasForeignKey(s => s.ProductionLineId);
        }
    }
}
