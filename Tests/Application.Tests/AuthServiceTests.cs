using System.Text.RegularExpressions;
using Core;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Tests;

public class AuthServiceTests
{
    protected class AuthServiceImplementation : AuthService
    {
        internal AuthServiceImplementation(IServiceProvider serviceProvider, IEnumerable<Type> authProviders) : base(serviceProvider, authProviders)
        {
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
    
    protected IServiceCollection ServiceProvider = new ServiceCollection();
    
    protected AuthServiceImplementation GetSut() => new AuthServiceImplementation(
        ServiceProvider.BuildServiceProvider(),
        new[]
        {
            typeof(FakeAuthProviderA),
            typeof(FakeAuthProviderB)
        }
    );

    public AuthServiceTests()
    {
        ServiceProvider.AddScoped<FakeAuthProviderA>();
        ServiceProvider.AddScoped<FakeAuthProviderB>();
    }
    
    [Fact]
    public void Wrong_provider_type_throw_InvalidArgumentException()
    {
        var act = () =>  new AuthServiceImplementation(
            ServiceProvider.BuildServiceProvider(),
            new[]
            {
                typeof(FakeAuthProviderA),
                typeof(FakeAuthProviderB),
                typeof(List<string>)
            }
        );

        act.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public async Task Invoke_Only_Exclusive_Provider()
    {
        var sut = GetSut();

        var credentials =  UserCredentials.From("test", "test"); // this credentials is present in FakeAuthProviderB but is considered FakeAuthProviderA exclusive 

        var result = await sut.LogInAsync(credentials); 
        
        result.Should().BeFalse();
    }
    
    [Fact]
    public async Task Service_logIn_success_with_existing_credentials()
    {
        var sut = GetSut();

        var credentials = UserCredentials.From("username", "password"); 

        var result = await sut.LogInAsync(credentials); 
        
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task Service_logIn_failure_with_non_existing_credentials()
    {
        var sut = GetSut();

        var credentials = UserCredentials.From("XXXX", "XXXX"); 

        var result = await sut.LogInAsync(credentials); 
        
        result.Should().BeFalse();
    }
}