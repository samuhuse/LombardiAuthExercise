using Application;
using LogIn.API.Configuration;
using RestAPI;
using RestAPI.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuthService, DefaultAuthService>();

builder.Services.AddLogInModule();
builder.Services.AddAuthProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapLogInModule();

app.Run();

public partial class Program{}



