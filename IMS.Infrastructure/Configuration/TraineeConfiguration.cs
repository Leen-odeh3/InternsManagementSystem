
using IMS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configuration;
public class TraineeConfiguration : IEntityTypeConfiguration<Trainee>
{
    public void Configure(EntityTypeBuilder<Trainee> builder)
    {
        builder.ToTable("Trainees");
        builder.HasKey(h => h.Id);
        builder.HasOne(h => h.User)
               .WithOne(u => u.Trainee)
               .HasForeignKey<Trainee>(h => h.Id);
    }
}