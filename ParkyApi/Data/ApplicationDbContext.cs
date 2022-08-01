using Microsoft.EntityFrameworkCore;
using ParkyApi.Data.Configurations;
using ParkyApi.Models;

namespace ParkyApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<NationalPark> NationalPark { get; set; }
    public DbSet<Trail> Trail { get; set; }
    public DbSet<User> User { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.SetQueryFilterOnAllEntities<IBaseEntity>(x => !x.Deleted);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NationalParkConfiguration).Assembly);
    }

    public DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class
    {
        var dbSet = base.Set<TEntity>();

        return dbSet;
    }
}