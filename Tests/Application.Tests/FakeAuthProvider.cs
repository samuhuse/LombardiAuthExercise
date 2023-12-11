using System.Text.RegularExpressions;
using Core;

namespace Application.Tests;

public abstract class FakeAuthProvider : IAuthProvider
{
    private readonly Regex _isExlusiveRegex;
    private readonly IEnumerable<UserCredentials> _credentials;

    protected FakeAuthProvider(Regex isExlusiveRegex, IEnumerable<UserCredentials> credentials)
    {
        _isExlusiveRegex = isExlusiveRegex;
        _credentials = credentials;
    }
    
    public Task<bool> IsExclusiveAsync(UserCredentials credentials, CancellationToken? cancellationToken)
    {
        return Task.FromResult(_isExlusiveRegex.Match(credentials.UserName).Success);
    }

    public Task<bool> LogInAsync(UserCredentials credentials, CancellationToken? cancellationToken)
    {
        return Task.FromResult(_credentials.Contains(credentials));
    }
}
