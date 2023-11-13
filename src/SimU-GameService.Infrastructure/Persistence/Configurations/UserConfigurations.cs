using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Infrastructure.Persistence.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // set the primary key
        builder.HasKey(u => u.UserId);
        builder.Property(u => u.UserId)
            .ValueGeneratedNever();
        builder.Ignore(b => b.Id);
        

        // location
        builder.HasOne(u => u.Location);

        // conversion for CreatedTime
        builder.Property(e => e.CreatedTime)
            .HasConversion(v => v.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        // friends
        builder.OwnsMany(u => u.Friends, f =>
        {
            f.WithOwner().HasForeignKey(nameof(User.UserId));
            f.Property(f => f.FriendId)
                .ValueGeneratedNever();
        });

        // question responses
        builder.OwnsMany(u => u.QuestionResponses, qr =>
        {
            qr.WithOwner().HasForeignKey(nameof(User.UserId));
        });
    }
}