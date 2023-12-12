using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using LogIn.API.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;

namespace RestAPI.Tests;

public class LogInTests
{
    [Theory]
    [InlineData("Samuele", "SuperPassword")]
    [InlineData("Omid", "PrettyPassword")]
    [InlineData("Cinzia", "StrongPassword")]
    [InlineData("Giovanni", "WeakPassword")]
    public async Task LogIn_respond_with_200_status_code_with_correct_credential(string userName, string password)
    {
        var request = new LogInRequestDto { UserName = userName, Password = password };
        await using var application = new WebApplicationFactory<Program>();
        var client = application.CreateClient();

        var result = await client.PostAsJsonAsync("/logIn", request);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task LogIn_respond_with_401_status_code_with_incorrect_credential()
    {
        var request = new LogInRequestDto { UserName = "XXXX", Password = "XXXX" };
        await using var application = new WebApplicationFactory<Program>();
        var client = application.CreateClient();

        var result = await client.PostAsJsonAsync("/logIn", request);

        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}