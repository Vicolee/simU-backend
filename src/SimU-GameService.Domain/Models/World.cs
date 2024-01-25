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

        [Column("WorldId")] // TO DO: REVIEW WITH LEKINA WHY WE NEED CHARACTER_ID IF WE INHERIT IT FROM ENTITY
        public Guid WorldId { get; private set; }
        public string? PrivateCode { get; set; }

        protected World()
        {
            UsersInWorld = new();
            AgentsInWorld = new();
            WorldId = Id;
        }

        protected World(
            string worldName,
            Guid ownerId,
            string description
        ) : this()
        {
            WorldName = WorldName;
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