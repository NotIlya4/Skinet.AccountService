using Infrastructure.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework;

public class AppDbContext : DbContext
{
    public DbSet<UserData> Users { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}