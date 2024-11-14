

using Domain.Aggregates.Products;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .HasConversion(pId => pId.Value, value => new ProductId(value))
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Gtin)
                .HasConversion(gtin => gtin.Gtin, value => new GTIN(value))
                .HasColumnType("integer");

            builder.Property(p => p.GtinGroup)
                .HasConversion(gtin => gtin!.Value.Gtin, value => new GTIN(value))
                .HasColumnType("integer");
                

            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Gtin).IsRequired();

            builder.HasIndex(p => p.Name)
                .IsUnique();

            builder.HasAlternateKey(p => p.Gtin);

            builder.HasMany(p => p.Sessions)
                .WithOne(s=>s.Product)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
