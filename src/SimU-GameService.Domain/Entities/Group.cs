using Microsoft.AspNetCore.Components.Routing;

namespace GameService.Models;
public class Group {
    public string? groupID { get; set; }
    public List<string>? users {get; set; }
    public string? name { get; set; }
    public DateTime createdAt { get; set; }

}
