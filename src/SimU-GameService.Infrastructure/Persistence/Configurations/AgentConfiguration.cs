using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Infrastructure.Persistence.Configurations;

public class AgentConfigurations : IEntityTypeConfiguration<Agent>
{
    public void Configure(EntityTypeBuilder<Agent> builder)
    {
        // set the primary key
        builder.HasKey(builder => builder.AgentId);

        // configure Guid properties to be user-generated
        builder.Ignore(b => b.Id);
        builder.Property(builder => builder.AgentId)
            .ValueGeneratedNever();
        builder.Property(builder => builder.UserId)
            .ValueGeneratedNever();

        // conversion for CreatedTime
        builder.Property(e => e.CreatedTime)
            .HasConversion(v => v.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
    }
}