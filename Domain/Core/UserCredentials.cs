using Ardalis.GuardClauses;

namespace Core;

public record UserCredentials
{
    public string UserName { get; }
    public string Password { get; }

    private UserCredentials(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }

    public static UserCredentials From(string userName, string password)
    {
        Guard.Against.NullOrEmpty(userName);
        Guard.Against.NullOrEmpty(password);

        return new(userName, password);
    }

    public virtual bool Equals(UserCredentials? other)
    {
        if (other is null) return false;
        
        string[] usernameSegments = other.UserName.Split('\\');

        return 
            string.Equals(UserName, usernameSegments[^1], StringComparison.OrdinalIgnoreCase) &&
            Password == other.Password;
    }
}