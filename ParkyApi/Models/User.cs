namespace ParkyApi.Models;

public class User : BaseEntity
{
    public string Username { get; private set; }
    public string Password { get; private set; }
    public string Role { get; set; }
    public string Token { get; private set; }

    public void Edit(string username, string password, string role)
    {
        Username = username;
        Password = password;
        Role = role;
        HasUpdated();
    }

    public void DefineToken(string token)
    {
        Token = token;
    }
}