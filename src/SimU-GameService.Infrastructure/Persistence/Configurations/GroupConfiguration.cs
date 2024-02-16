using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Infrastructure.Persistence.Configurations;

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        // set primary key
        builder.HasKey(builder => builder.Id);

        // configure Guid properties to be user-generated
               
        builder.Property(i => i.Id)
            .ValueGeneratedNever();
        builder.Property(i => i.OwnerId)
            .ValueGeneratedNever();

        // conversion for CreatedTime
        builder.Property(e => e.CreatedTime)
            .HasConversion(v => v.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        // memberIds
        builder.Property(e => e.MemberIds)
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