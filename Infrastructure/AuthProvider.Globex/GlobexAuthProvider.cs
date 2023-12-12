using Application;
using Core;

namespace AuthProvider.Globex;

public class GlobexAuthProvider : IAuthProvider
{
    private readonly List<UserCredentials> _userCredentials = new()
    {
        UserCredentials.From("Cinzia", "StrongPassword")
    };
    
    public Task<bool> IsExclusiveAsync(UserCredentials credentials, CancellationToken? cancellationToken)
    {
        return Task.FromResult(false);
    }

    public Task<bool> LogInAsync(UserCredentials credentials, CancellationToken? cancellationToken)
    {
        // Emulating auth
        return Task.FromResult(_userCredentials.Contains(credentials));
    }
}