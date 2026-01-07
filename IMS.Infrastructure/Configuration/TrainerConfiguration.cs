

using IMS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configuration;
public class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
{
    public void Configure(EntityTypeBuilder<Trainer> builder)
    {
        builder.ToTable("Trainers");
        builder.HasKey(h => h.Id);
        builder.HasOne(h => h.User)
               .WithOne(u => u.Trainer)
               .HasForeignKey<Trainer>(h => h.Id);
    }
}
