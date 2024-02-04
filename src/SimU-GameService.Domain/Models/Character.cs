using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SimU_GameService.Domain.Primitives;

namespace SimU_GameService.Domain.Models
{
    public class Character : Entity
    {
        public string? Username { get; set; }
        public DateTime CreatedTime { get; set; }
        public int LastKnownX { get; set; }
        public int LastKnownY { get; set; }
        public string? Summary { get; set; }
        public Uri? Sprite { get; set;}
        public Uri? SpriteHeadshot { get; set;}

        [Column("CharacterId")] // TO DO: REVIEW WITH LEKINA WHY WE NEED CHARACTER_ID IF WE INHERIT IT FROM ENTITY
        public Guid CharacterId { get; private set; }

        protected Character()
        {
            CharacterId = Id;
            LastKnownX = 0;
            LastKnownY = 0;
        }

        protected Character(
            string username,
            Uri sprite,
            string summary,
            Uri spriteHeadshot
        ) : this()
        {
            Username = username;
            Summary = summary;
            CreatedTime = DateTime.UtcNow;
            Sprite = sprite;
            SpriteHeadshot = spriteHeadshot;
        }

        public void UpdateLocation(int xCoord, int yCoord)
        {
                LastKnownX = xCoord;
                LastKnownY = yCoord;
        }
    }
}