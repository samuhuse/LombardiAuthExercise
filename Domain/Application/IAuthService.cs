using Core;

namespace Application;

public interface IAuthService
{
    Task<bool> LogInAsync(UserCredentials credentials, CancellationToken? cancellationToken);
}