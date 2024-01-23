using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SimU_GameService.Domain.Primitives;

namespace SimU_GameService.Domain.Models
{
    public class Character : Entity
    {
        public string? Username { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedTime { get; set; }
        public Location? Location { get; set; }
        public string? Sprite { get; set;}
        public string? SpriteHeadshot { get; set;}

        [Column("CharacterId")] // TO DO: REVIEW WITH LEKINA WHY WE NEED CHARACTER_ID IF WE INHERIT IT FROM ENTITY
        public Guid CharacterId { get; private set; }

        protected Character()
        {
            Location = new Location
            {
                LocationId = Guid.NewGuid(),
                X = 0,
                Y = 0,
            };
            CharacterId = Id;
        }

        protected Character(
            string username,
            string description,
            string sprite,
            string spriteHeadshot
        ) : this()
        {
            Username = username;
            Description = description;
            CreatedTime = DateTime.UtcNow;
            Sprite = sprite;
            SpriteHeadshot = spriteHeadshot;
        }

        public void UpdateLocation(int xCoord, int yCoord)
        {
            Location = new Location
            {
                LocationId = Guid.NewGuid(),
                X = xCoord,
                Y = yCoord
            };
        }
    }
}