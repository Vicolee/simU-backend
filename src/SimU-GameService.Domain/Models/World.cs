using System;
using System.ComponentModel.DataAnnotations.Schema;
using SimU_GameService.Domain.Primitives;

namespace SimU_GameService.Domain.Models
{
    public class World : Entity
    {
        public string? WorldName { get; set; }
        public Guid OwnerId { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedTime { get; set; }
        public List<Guid> UsersInWorld { get; set; }
        public List<Guid> AgentsInWorld { get; set; }
        public Uri? Thumbnail { get; set;}

        [Column("WorldId")] // TO DO: REVIEW WITH LEKINA WHY WE NEED CHARACTER_ID IF WE INHERIT IT FROM ENTITY
        public Guid WorldId { get; private set; }
        public string? PrivateCode { get; set; }

        public World()
        {
            UsersInWorld = new();
            AgentsInWorld = new();
            WorldId = Id;
        }

        public World(
            string worldName,
            string description,
            Guid ownerId
        ) : this()
        {
            WorldName = worldName;
            OwnerId = ownerId;
            Description = description;
            CreatedTime = DateTime.UtcNow;
            GeneratePrivateCode();
        }
    private void GeneratePrivateCode()
    {
       // while loop here to generate private code
    }
    }
}