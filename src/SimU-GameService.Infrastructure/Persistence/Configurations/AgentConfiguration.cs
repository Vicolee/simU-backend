using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Infrastructure.Persistence.Configurations;

public class AgentConfigurations : IEntityTypeConfiguration<Agent>
{
    public void Configure(EntityTypeBuilder<Agent> builder)
    {
        // set the primary key
        builder.HasKey(builder => builder.Id);

        // configure Guid properties to be user-generated
        builder.Property(builder => builder.Id)
            .ValueGeneratedNever();

        // conversion for CreatedTime
        builder.Property(e => e.CreatedTime)
            .HasConversion(v => v.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        // question responses
        // builder.OwnsMany(a => a.QuestionResponses, qr =>
        // {
        //     qr.WithOwner().HasForeignKey(nameof(Agent.Id));
        // });

        // location
        
        // location
        builder.OwnsOne(
            a => a.Location,
            l =>
            {
                l.Property(x => x.X_coord).HasColumnName("X_coord");
                l.Property(y => y.Y_coord).HasColumnName("Y_coord");
            });
    }
}