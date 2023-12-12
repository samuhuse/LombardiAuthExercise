namespace LogIn.API.Configuration;

public static class WebApplicationExtensions
{
    public static WebApplication MapLogInModule(this WebApplication app)
    {
        app.MapCarter();

        return app;
    }
}