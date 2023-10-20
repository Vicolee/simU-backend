namespace SimU_GameService.Domain.Entities;

public class User : Entity
{
    public string Username { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public User(string username, string email, string password) : base()
    {
        Username = username;
        Email = email;
        Password = password;
    }
}
