

using Domain.Aggregates.MarkingCodes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal class MarkingCodeConfiguration : IEntityTypeConfiguration<MarkingCodeEntity>
    {
        public void Configure(EntityTypeBuilder<MarkingCodeEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasConversion(xId => xId.Value, value => new(value))
                .ValueGeneratedOnAdd();

            builder.Property(x => x.ParentId)
                .HasConversion(xParentId => xParentId!.Value.Value, value => new(value))
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Code)
                .HasConversion(x => x.Code, code => new(code))
                .HasColumnType("TEXT");

            builder.Property(x => x.Code)
                .HasMaxLength(60)
                .IsRequired();

            builder.HasAlternateKey(x => x.Code);


            builder.HasOne(x => x.Session)
                .WithMany(s => s.MarkingCodes)
                .HasForeignKey(x => x.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(c => c.CodeType)
                .IsRequired();
        }

    }
}
