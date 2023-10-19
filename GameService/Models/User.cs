using Microsoft.AspNetCore.Components.Routing;

namespace GameService.Models;

public class User
{
    public string? userID { get; set; }
    public string? firebaseID { get; set; }
    public string? username { get; set; }
    public string? email { get; set; }
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