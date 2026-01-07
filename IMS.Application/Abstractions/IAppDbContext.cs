using IMS.Core.Entities;
using Microsoft.EntityFrameworkCore;


namespace IMS.Application.Abstractions;
public interface IAppDbContext
{
    DbSet<Trainee> Trainees { get; }
    DbSet<Trainer> Trainers { get; }
    DbSet<Manager> Managers { get; }
    DbSet<Hr> Hrs { get; }
    DbSet<Admin> Admins { get; }
    DbSet<TeamLead> TeamLeads { get; }  
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}