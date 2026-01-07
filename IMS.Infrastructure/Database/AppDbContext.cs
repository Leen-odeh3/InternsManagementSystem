
using IMS.Application.Abstractions;
using IMS.Core.Entities;
using IMS.Infrastructure.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Emit;

namespace IMS.Infrastructure.Database;

public class AppDbContext : IdentityDbContext<AppUser,AppRole,int>, IAppDbContext
{
    public DbSet<Trainee> Trainees { get; set; }

    public DbSet<Trainer> Trainers { get; set; }
    public DbSet<Manager> Managers { get; set; }

    public DbSet<Hr> Hrs { get; set; }

    public DbSet<Admin> Admins { get; set; }
    public DbSet<TeamLead> TeamLeads { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new TrainerConfiguration());
        builder.ApplyConfiguration(new TraineeConfiguration());
        builder.ApplyConfiguration(new HRConfiguration());
        builder.ApplyConfiguration(new ManagerConfiguration());
        builder.ApplyConfiguration(new TeamLeadConfiguration());
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}
