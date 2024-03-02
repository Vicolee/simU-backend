using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Infrastructure.Persistence.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // set the primary key
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .ValueGeneratedNever();
       
        
        // location
        builder.OwnsOne(
            u => u.Location,
            l =>
            {
                l.Property(x => x.X_coord).HasColumnName("X_coord");
                l.Property(y => y.Y_coord).HasColumnName("Y_coord");
            });

        // conversion for CreatedTime
        builder.Property(e => e.CreatedTime)
            .HasConversion(v => v.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        // friends
        builder.OwnsMany(u => u.Friends, f =>
        {
            f.WithOwner().HasForeignKey(nameof(User.Id));
            f.Property(f => f.FriendId)
                .ValueGeneratedNever();
        });

        // question responses
        // builder.OwnsMany(u => u.QuestionResponses, qr =>
        // {
        //     qr.WithOwner().HasForeignKey(nameof(User.Id));
        // });

        // worldsJoined
        builder.Property(e => e.WorldsJoined)
            .HasConversion(
                m => JsonConvert.SerializeObject(m),
                m => JsonConvert.DeserializeObject<List<Guid>>(m) ?? new List<Guid>())
            .Metadata.SetValueComparer(
                new ValueComparer<List<Guid>>(
                    (c1, c2)
                        => (c1 == null && c2 == null) || (c1 != null && c2 != null && c1.SequenceEqual(c2)),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));
        
        // worldsCreated
        builder.Property(e => e.WorldsCreated)
            .HasConversion(
                m => JsonConvert.SerializeObject(m),
                m => JsonConvert.DeserializeObject<List<Guid>>(m) ?? new List<Guid>())
            .Metadata.SetValueComparer(
                new ValueComparer<List<Guid>>(
                    (c1, c2)
                        => (c1 == null && c2 == null) || (c1 != null && c2 != null && c1.SequenceEqual(c2)),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));
    }
}