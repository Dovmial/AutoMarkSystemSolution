using AmsAPI.Autorize.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmsAPI.Autorize.Data.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasConversion(r => r.Value, value => new(value))
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Name).IsRequired();
        }
    }
}
