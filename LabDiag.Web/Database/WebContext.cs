using LabDiag.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace LabDiag.Web.Database;

public class WebContext(DbContextOptions<WebContext> options) : DbContext(options)
{
    public DbSet<Nic> Nic { get; set; }
    
    public DbSet<Computer> Computer { get; set; }
    
    public DbSet<Lab> Lab { get; set; }
    
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Unique indexes
        modelBuilder.Entity<Computer>()
            .HasIndex(b => b.HostName).IsUnique();
        modelBuilder.Entity<Lab>()
            .HasIndex(b => b.Name).IsUnique();
        
        modelBuilder.Entity<Computer>()
            .HasMany(e => e.Nic)
            .WithOne(e => e.Computer)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Lab>()
            .HasMany(e => e.Computers)
            .WithOne(e => e.Lab)
            .OnDelete(DeleteBehavior.Cascade);

    }
}

