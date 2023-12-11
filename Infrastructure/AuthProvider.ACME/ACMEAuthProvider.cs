using System.Text.RegularExpressions;
using Application;
using Core;

namespace AuthProvider.ACME;

public class ACMEAuthProvider : IAuthProvider
{
    private readonly Regex _isExlusiveRegex = new Regex(@"^acme", RegexOptions.IgnoreCase);

    private readonly List<UserCredentials> _userCredentials = new()
    {
        new("Samuele", "SuperPassword"),
        new("Giovanni","WeakPassword")
    };
    
    public Task<bool> IsExclusiveAsync(UserCredentials credentials, CancellationToken? cancellationToken)
    {
        return Task.FromResult(_isExlusiveRegex.Match(credentials.UserName).Success);
    }

    public Task<bool> LogInAsync(UserCredentials credentials, CancellationToken? cancellationToken)
    {
        // Emulating auth
        return Task.FromResult(_userCredentials.Contains(credentials));
    }
}