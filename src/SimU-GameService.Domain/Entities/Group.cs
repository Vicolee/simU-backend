namespace SimU_GameService.Domain
{
    public class Group {
        public string? groupID { get; set; }
        public List<string>? users {get; set; }
        public string? name { get; set; }
        public DateTime createdAt { get; set; }

    }
}
