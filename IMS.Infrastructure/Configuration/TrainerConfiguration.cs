

using IMS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configuration;
public class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
{
    public void Configure(EntityTypeBuilder<Trainer> builder)
    {
        builder.ToTable("Trainers");
        builder.HasKey(t => t.UserId);
        builder.HasOne<AppUser>()
               .WithOne()
               .HasForeignKey<Trainer>(t => t.UserId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);
    }
}
