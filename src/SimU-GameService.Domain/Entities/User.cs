using System.ComponentModel.DataAnnotations;

namespace SimU_GameService.Domain;

public class User
{
    public Guid userID { get; set; }
    [Required]
    public string? firebaseID { get; set; }
    public string? username { get; set; }
    [Required]
    [MaxLength(32)]
    [MinLength(2)]
    public string? email { get; set; }
    [Required]
    public DateTime createdAt { get; set; }
    public List<Friend>? friends { get; set; }
    public Location? lastLocation { get; set; }
    public string? map_id { get; set; }

    // public Memory memories { get; set; }

    // public Personality personality { get; set; }



}

public class Friend
{
    public string userID { get; set; }
    public DateTime befriendedAt { get; set; }
}

public class Location
{
    public int x { get; set; }
    public int y { get; set; }
}