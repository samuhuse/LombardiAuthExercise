using Core;

namespace Application;

public class AuthServiceChain : IAuthProvider
{
    public class Builder
    {
        private readonly AuthServiceChain _rootChain;
        private AuthServiceChain _currentChainElement;
        
        private Builder(AuthServiceChain rootChain)
        {
            _rootChain = rootChain;
            _currentChainElement = rootChain;
        }

        public static Builder CreateChain(IAuthProvider provider)
        {
            var chain = new AuthServiceChain(provider, new SharedState());
            return new Builder(chain);
        }

        public Builder AddNext(IAuthProvider provider)
        {
            var nextChainElement = new AuthServiceChain(provider, _rootChain.State);
            _currentChainElement.SetNext(nextChainElement);
            _currentChainElement = nextChainElement;

            return this;
        }

        public AuthServiceChain Build() => _rootChain;
    }

    protected class SharedState
    {
        public bool ExlusiveSearched = false;
        public bool ThereIsExlusive = false;
    }
    
    private readonly IAuthProvider _provider;
    private AuthServiceChain? _next;

    protected readonly SharedState State;

    private bool _isExclusiveProvider = false;

    private AuthServiceChain(IAuthProvider provider, SharedState state)
    {
        _provider = provider;
        State = state;
    }

    public AuthServiceChain SetNext(AuthServiceChain next)
    {
        _next = next;
        return next;
    }
    
    public Task<bool> IsExclusiveAsync(UserCredentials credentials, CancellationToken? cancellationToken)
    {
        return _provider.IsExclusiveAsync(credentials, cancellationToken);
    }

    public async Task<bool> LogInAsync(UserCredentials credentials, CancellationToken? cancellationToken)
    {
        if (!State.ExlusiveSearched)
        {
            State.ThereIsExlusive = await ThereIsExclusive(credentials, cancellationToken);
            State.ExlusiveSearched = true;
        }

        if(State.ThereIsExlusive && _isExclusiveProvider)
            return await _provider.LogInAsync(credentials, cancellationToken);
        
        else if(State.ThereIsExlusive) return await (_next?.LogInAsync(credentials, cancellationToken) ?? Task.FromResult(false));
        
        else if (await _provider.LogInAsync(credentials, cancellationToken)) return true;
        
        return await (_next?.LogInAsync(credentials, cancellationToken) ?? Task.FromResult(false));
    }
    
    private async Task<bool> ThereIsExclusive(UserCredentials credentials, CancellationToken? cancellationToken)
    {
        if (await _provider.IsExclusiveAsync(credentials, cancellationToken))
        {
            _isExclusiveProvider = true;
            return true;
        }
        return await (_next?.IsExclusiveAsync(credentials, cancellationToken) ?? Task.FromResult(false));
    }


}