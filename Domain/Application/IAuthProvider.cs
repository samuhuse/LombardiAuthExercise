using Core;

namespace Application;

public interface IAuthProvider
{
    Task<bool> IsExclusiveAsync(UserCredentials credentials, CancellationToken? cancellationToken);
    Task<bool> LogInAsync(UserCredentials credentials, CancellationToken? cancellationToken);
}