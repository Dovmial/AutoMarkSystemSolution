using AmsAPI.Autorize.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AmsAPI.Autorize.Data.Configuration
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasConversion(xId => xId.Value, value => new (value))
                .ValueGeneratedNever()
                .HasColumnType("TEXT");

            builder.Property(a => a.Login).IsRequired();
            builder.HasAlternateKey(a => a.Login);

            builder.HasMany(a => a.Roles)
                .WithMany(r => r.Accounts)
                .UsingEntity(t => t.ToTable("AccountsRoles"));
        }
    }
}
