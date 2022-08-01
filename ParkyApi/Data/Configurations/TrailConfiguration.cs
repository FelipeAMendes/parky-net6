using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkyApi.Models;

namespace ParkyApi.Data.Configurations;

public class TrailConfiguration : IEntityTypeConfiguration<Trail>
{
    public void Configure(EntityTypeBuilder<Trail> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.HasOne(x => x.NationalPark).WithMany(x => x.Trails);
    }
}