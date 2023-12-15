using System.Text.RegularExpressions;
using Core;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Tests;

public class AuthServiceTests
{
    protected class AuthServiceImplementation : IAuthService
    {
        private readonly AuthProviderChain _chain = AuthProviderChain.Builder
            .CreateChain(new FakeAuthProviderA())
            .AddNext(new FakeAuthProviderB())
            .Build();

        public Task<bool> LogInAsync(UserCredentials credentials, CancellationToken? cancellationToken)
        {
            return _chain.LogInAsync(credentials, cancellationToken);
        }
    }

    protected class FakeAuthProviderA : FakeAuthProvider
    {
        public FakeAuthProviderA() 
            : base(
                new Regex(@"^test", RegexOptions.IgnoreCase), 
                new List<UserCredentials>() {UserCredentials.From("testA","testA")}
                ) { }
    }
    
    protected class FakeAuthProviderB : FakeAuthProvider
    {
        public FakeAuthProviderB() 
            : base(
                new Regex(@"^noMatch"), 
                new List<UserCredentials>()
                {
                    UserCredentials.From("test","test"),
                    UserCredentials.From("username","password")
                }
            ) { }
    }

    
    
    [Fact]
    public async Task Invoke_Only_Exclusive_Provider()
    {
        var sut = new AuthServiceImplementation();

        var credentials = UserCredentials.From("username", "password"); 

        var result = await sut.LogInAsync(credentials, null); 
        
        result.Should().BeTrue();
        
         sut = new AuthServiceImplementation();

         credentials =  UserCredentials.From("test", "test"); // this credentials is present in FakeAuthProviderB but is considered FakeAuthProviderA exclusive 

         result = await sut.LogInAsync(credentials, null); 
        
        result.Should().BeFalse();
    }
    
    [Fact]
    public async Task Service_logIn_success_with_existing_credentials()
    {
        var sut = new AuthServiceImplementation();

        var credentials = UserCredentials.From("username", "password"); 

        var result = await sut.LogInAsync(credentials, null); 
        
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task Service_logIn_failure_with_non_existing_credentials()
    {
        var sut = new AuthServiceImplementation();

        var credentials = UserCredentials.From("XXXX", "XXXX"); 

        var result = await sut.LogInAsync(credentials, null); 
        
        result.Should().BeFalse();
    }
}