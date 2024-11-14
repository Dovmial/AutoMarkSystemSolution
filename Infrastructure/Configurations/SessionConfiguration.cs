

using Domain.Aggregates.Sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal class SessionConfiguration : IEntityTypeConfiguration<SessionEntity>
    {
        public void Configure(EntityTypeBuilder<SessionEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasConversion(xId => xId.Value, value => new SessionId(value))
                .HasColumnType("TEXT");
                
            builder.Property(x => x.ProductId)
                .IsRequired();

            builder.Property(x => x.ProductionDate)
                .IsRequired();

            builder.HasOne(x => x.ProductionLine)
                .WithMany(pl => pl.Sessions)
                .HasForeignKey(x => x.ProductionLineId)
                .OnDelete(DeleteBehavior.SetNull);
           
        }
    }
}
