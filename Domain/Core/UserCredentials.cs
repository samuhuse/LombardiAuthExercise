using Ardalis.GuardClauses;

namespace Core;

public record UserCredentials(string UserName, string Password)
{
    public static UserCredentials From(string userName, string password)
    {
        Guard.Against.NullOrEmpty(userName);
        Guard.Against.NullOrEmpty(password);

        return new(userName, password);
    }
}