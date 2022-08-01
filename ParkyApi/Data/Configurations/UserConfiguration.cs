using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkyApi.Models;

namespace ParkyApi.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Username).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Password).HasMaxLength(256).IsRequired();
        builder.Property(x => x.Role).HasMaxLength(50).IsRequired();
        builder.Ignore(x => x.Token);
    }
}
