
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
    public DbSet<Trainee> Trainees => throw new NotImplementedException();

    public DbSet<Trainer> Trainers => throw new NotImplementedException();

    public DbSet<Manager> Managers => throw new NotImplementedException();

    public DbSet<Hr> Hrs => throw new NotImplementedException();

    public DbSet<Admin> Admins => throw new NotImplementedException();

    public DbSet<TeamLead> TeamLeads => throw new NotImplementedException();

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
