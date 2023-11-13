using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Infrastructure.Persistence.Configurations;

public class ChatConfigurations : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        // set the primary key
        builder.HasKey(builder => builder.Id);

        // configure Guid properties to be user-generated
        builder.Property(builder => builder.Id)
            .ValueGeneratedNever();
        builder.Property(builder => builder.SenderId)
            .ValueGeneratedNever();
        builder.Property(builder => builder.RecipientId)
            .ValueGeneratedNever();

        // conversion for CreatedTime
        builder.Property(e => e.CreatedTime)
            .HasConversion(v => v.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
    }
}