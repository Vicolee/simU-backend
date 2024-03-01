using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Infrastructure.Persistence.Configurations;

public class ConversationConfigurations : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        // set the primary key
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        // conversion for CreatedTime
        builder.Property(e => e.CreatedTime)
            .HasConversion(v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        // conversion for LastMessageSentAt
        builder.Property(e => e.LastMessageSentAt)
            .HasConversion(v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
    }
}