using Application;
using Carter.ModelBinding;
using Carter.OpenApi;
using Carter.Response;
using Core;
using LogIn.API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LogIn.API.Routing;

public class LogInModule: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/logIn", LogIn)
            .Accepts<LogInRequestDto>("application/json")
            .Produces(200)
            .Produces<IEnumerable<ModelError>>(422)
            .WithTags("Auth")
            .WithName("LogIn")
            .IncludeInOpenApi();
    }

    public static async Task LogIn(IAuthService authService, HttpContext ctx, [FromBody]LogInRequestDto dto, CancellationToken cancellationToken)
    {
         var validationResult = ctx.Request.Validate(dto);
         if (!validationResult.IsValid)
         {
             ctx.Response.StatusCode = 422;
             await ctx.Response.Negotiate(validationResult.GetFormattedErrors(), cancellationToken);
             return;
         }
 
         if (!await authService.LogInAsync(credentials: dto, cancellationToken))
         {
             ctx.Response.StatusCode = 401;
             await ctx.Response.Negotiate("Invalid username or password. Please try again", cancellationToken);
         }
 
         ctx.Response.StatusCode = 200;
         await ctx.Response.Negotiate("You have logged in successfully", cancellationToken);
     }
    
}