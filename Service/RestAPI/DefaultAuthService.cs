using System;
using Application;
using AuthProvider.ACME;
using AuthProvider.Globex;
using AuthProvider.MassiveDynamic;

namespace RestAPI;

public class DefaultAuthService : AuthService  
{  
    //  Invoked in this sequence  
    static readonly Type[] AuthProviders = new[]  
    {  
        typeof(ACMEAuthProvider),  
        typeof(MassiveDynamicAuthProvider),  
        typeof(GlobexAuthProvider)  
        // ...  
    };  
    
    public DefaultAuthService(IServiceProvider serviceProvider) : base(serviceProvider, AuthProviders) { }
}