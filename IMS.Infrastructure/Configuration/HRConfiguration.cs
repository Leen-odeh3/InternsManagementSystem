using IMS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configuration;
public class HRConfiguration: IEntityTypeConfiguration<Hr>
{
    public void Configure(EntityTypeBuilder<Hr> builder)
    {
        builder.ToTable("Hrs");
        builder.HasKey(h => h.Id);
        builder.HasOne(h => h.User)
               .WithOne(u => u.Hr)
               .HasForeignKey<Hr>(h => h.Id);
    }
}