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
    
    public virtual bool Equals(UserCredentials? other) => string.Equals(UserName,other?.UserName,StringComparison.OrdinalIgnoreCase) && Password == other?.Password;
}