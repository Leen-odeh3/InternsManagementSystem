
using IMS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configuration;
public class TraineeConfiguration : IEntityTypeConfiguration<Trainee>
{
    public void Configure(EntityTypeBuilder<Trainee> builder)
    {
        builder.ToTable("Trainees");
        builder.HasKey(h => h.UserId);
        builder.HasOne(h => h.User)
               .WithOne()
               .HasForeignKey<Trainee>(h => h.UserId);
    }
}