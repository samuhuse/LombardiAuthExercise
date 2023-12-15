using System;
using Application;
using AuthProvider.ACME;
using AuthProvider.Globex;
using AuthProvider.MassiveDynamic;
using Core;

namespace RestAPI;

public class DefaultAuthService : IAuthService
{
    private readonly AuthProviderChain _chain = AuthProviderChain.Builder
        .CreateChain(new ACMEAuthProvider())
        .AddNext(new MassiveDynamicAuthProvider())
        .AddNext(new GlobexAuthProvider())
        .Build();

    public Task<bool> LogInAsync(UserCredentials credentials, CancellationToken? cancellationToken)
    {
        return _chain.LogInAsync(credentials, cancellationToken);
    }
}