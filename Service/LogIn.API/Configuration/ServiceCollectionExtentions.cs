using Application;
using Microsoft.Extensions.DependencyInjection;

namespace LogIn.API.Configuration;

public static class ServiceCollectionExtentions
{
    public static IServiceCollection AddLogInModule(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddCarter();

        return serviceCollection;
    }
}