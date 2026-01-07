
using IMS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configuration;
public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
{
    public void Configure(EntityTypeBuilder<Manager> builder)
    {
        builder.ToTable("Managers");
        builder.HasKey(h => h.Id);
        builder.HasOne(h => h.User)
               .WithOne(u => u.Manager)
               .HasForeignKey<Manager>(h => h.Id);
    }
}