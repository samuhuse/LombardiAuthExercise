using System;
using System.Linq;
using System.Reflection;
using Application;
using AuthProvider.ACME;
using AuthProvider.Globex;
using AuthProvider.MassiveDynamic;
using Microsoft.Extensions.DependencyInjection;

namespace RestAPI.Configuration;

public static class ServiceCollectionExtensions  
{  
    public static IServiceCollection AddAuthProviders(this IServiceCollection serviceCollection)
    {
        ForceInfrastructureAssembliesLoading();
        
        var authProviderTypes =   
            AppDomain.CurrentDomain.GetAssemblies()  
                .SelectMany(assembly => assembly.GetTypes())  
                .Where(type => typeof(IAuthProvider).IsAssignableFrom(type) && type is { IsInterface: false, IsAbstract: false });  
        
        foreach (var authProviderType in authProviderTypes) serviceCollection.AddScoped(authProviderType);
        
        return serviceCollection;  
    }

    private static void ForceInfrastructureAssembliesLoading()
    {
        foreach (var name in Directory.GetFiles(
                     AppDomain.CurrentDomain.BaseDirectory, "*.dll").Where(r => 
                     !AppDomain.CurrentDomain.GetAssemblies().ToList().Select(a => 
                         a.Location).ToArray().Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList())
        {
            Assembly.Load(Path.GetFileNameWithoutExtension(new FileInfo(name).Name));
        }
    }
    
}