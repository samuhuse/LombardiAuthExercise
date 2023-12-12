using System.Text.RegularExpressions;
using Application;
using Core;

namespace AuthProvider.MassiveDynamic;

public class MassiveDynamicAuthProvider : IAuthProvider
{
    private readonly List<UserCredentials> _userCredentials = new()
    {
        UserCredentials.From("Omid", "PrettyPassword")
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