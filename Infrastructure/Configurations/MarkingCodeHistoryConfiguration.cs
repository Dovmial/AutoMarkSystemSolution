

using Domain.Aggregates.MarkingCodeHistory;
using Domain.Aggregates.MarkingCodes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal class MarkingCodeHistoryConfiguration : IEntityTypeConfiguration<MarkingCodeHistoryEntity>
    {
        public void Configure(EntityTypeBuilder<MarkingCodeHistoryEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasConversion(xId => xId.Value, value => new(value))
                .ValueGeneratedOnAdd();

            builder.HasOne(h => h.MarkingCodeEntity)
                .WithMany(m => m.MarkingCodeHistoryEntities)
                .HasForeignKey(h => h.MarkingCodeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(h => h.TimeStamp).IsRequired();
        }
    }
}
