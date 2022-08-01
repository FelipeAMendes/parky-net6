using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkyApi.Models;

namespace ParkyApi.Data.Configurations;

public class NationalParkConfiguration : IEntityTypeConfiguration<NationalPark>
{
    public void Configure(EntityTypeBuilder<NationalPark> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.Property(x => x.State).HasMaxLength(100).IsRequired();
    }
}