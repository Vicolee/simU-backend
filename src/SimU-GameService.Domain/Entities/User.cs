using System;
using System.ComponentModel.DataAnnotations;

namespace SimU_GameService.Domain
{

    public class User
    {
        public Guid Id { get; set; }
        public string? firebaseID { get; set; }
        public string? username { get; set; }
        [Required]
        [MaxLength(32)]
        [MinLength(2)]
        public string? email { get; set; }
        [Required]

        public string? password { get; set;}
        [Required]
        public DateTime createdAt { get; set; }
        public List<Friend>? friends { get; set; }
        public Location? lastLocation { get; set; }
        public string? map_id { get; set; }

        // public Memory memories { get; set; }

        // public Personality personality { get; set; }

        public User(){}
        public User(string givenUsername, string givenEmail, string givenPass)
        {
           Id = Guid.NewGuid();
           username = givenUsername;
           email = givenEmail;
           password = givenPass;
        }

    }

    public class Friend
    {
        public Guid Id { get; set; }
        public DateTime befriendedAt { get; set; }
    }

    public class Location
    {
        public Guid Id {get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }
}