
using IMS.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace IMS.Infrastructure.Database;

public class AppDbContext : IdentityDbContext<AppUser,AppRole,int>
{
    public DbSet<Trainee> Trainees { get; set; }

    public DbSet<Trainer> Trainers { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}
