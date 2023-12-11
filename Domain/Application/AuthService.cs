using Ardalis.GuardClauses;
using Core;
using Microsoft.Extensions.DependencyInjection;
// ReSharper disable PossibleMultipleEnumeration

namespace Application;

public abstract class AuthService : IAuthService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly List<IAuthProvider> _authProviders = new();
    
    protected AuthService(System.IServiceProvider serviceProvider, IEnumerable<Type> authProviders)
    {
        Guard.Against.Null(serviceProvider);
        Guard.Against.Null(authProviders);
        
        _serviceProvider = serviceProvider;

        InitializeServiceProvidersInstances(authProviders);
    }

    public async Task<bool> LogInAsync(UserCredentials credentials, CancellationToken? cancellationToken)
    {
        Guard.Against.Null(credentials);

        if (await GetExclusiveProvider(credentials, cancellationToken) is { } exclusiveProvider)
            return await exclusiveProvider.LogInAsync(credentials, cancellationToken);

        foreach (var authProvider in _authProviders)
            if (await authProvider.LogInAsync(credentials, cancellationToken)) return true;

        return false;
    }
    
    private void InitializeServiceProvidersInstances(IEnumerable<Type> authProviders)
    {
        if (!authProviders.All(authProvider => authProvider.IsAssignableTo(typeof(IAuthProvider))))
            throw new ArgumentException($"{nameof(authProviders)} not implementing {nameof(IAuthProvider)}", nameof(authProviders));

        foreach (var authProvider in authProviders)
            _authProviders.Add((IAuthProvider)_serviceProvider.GetRequiredService(authProvider));
    }

    private async Task<IAuthProvider?> GetExclusiveProvider(UserCredentials credentials, CancellationToken? cancellationToken) // Could use a Maybe Monad
    {
        foreach (var authProvider in _authProviders)
            if (await authProvider.IsExclusiveAsync(credentials, cancellationToken)) return authProvider;

        return null;
    }
}