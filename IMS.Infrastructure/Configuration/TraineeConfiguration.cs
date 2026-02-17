
using IMS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configuration;
public class TraineeConfiguration : IEntityTypeConfiguration<Trainee>
{
    public void Configure(EntityTypeBuilder<Trainee> builder)
    {
        builder.ToTable("Trainees");
        builder.HasKey(t => t.UserId);
        builder.HasOne<AppUser>()
               .WithOne()
               .HasForeignKey<Trainee>(t => t.UserId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);
    }
}