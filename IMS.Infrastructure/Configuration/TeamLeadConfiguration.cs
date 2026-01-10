

using IMS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Configuration;

public class TeamLeadConfiguration : IEntityTypeConfiguration<TeamLead>
{
    public void Configure(EntityTypeBuilder<TeamLead> builder)
    {
        builder.ToTable("TeamLeads");
        builder.HasKey(h => h.Id);
        builder.HasOne(h => h.User)
               .WithOne(u => u.TeamLead)
               .HasForeignKey<TeamLead>(h => h.Id);
    }
}