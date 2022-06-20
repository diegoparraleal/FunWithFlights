using CSharpFunctionalExtensions;
using FunWithFlights.Core.Extensions;
using FunWithFlights.Core.Time;

namespace FunWithFlights.Core.Cache;

public interface ICachedResult<T>
{
    Task<T> GetAsync();
}

public class CachedResult<T>: ICachedResult<T>
{
    private readonly TimeSpan _toExpire;
    private readonly Func<Task<T>> _callback;
    private readonly IClock _clock;
    private Maybe<T> _maybeResult;
    private Maybe<DateTime> _maybeLastExecution;
    private readonly SemaphoreSlim _semaphoreSlim = new (1, 1);
    
    public CachedResult(TimeSpan toExpire, Func<Task<T>> callback, IClock? clock = null)
    {
        _toExpire = toExpire;
        _callback = callback;
        _clock = clock ?? new Clock();
    }

    public async Task<T> GetAsync()
    {
        if (!_maybeResult.TryGet(out var result)) return await RefreshAsync();
        if (!_maybeLastExecution.TryGet(out var lastExecution)) return await RefreshAsync();
        if (lastExecution + _toExpire < DateTime.Now) return await RefreshAsync();
        return result;
    }

    private async Task<T> RefreshAsync()
    {
        await _semaphoreSlim.WaitAsync();
        try
        {
            _maybeResult = await _callback();
            _maybeLastExecution = DateTime.Now;
        
            return _maybeResult.GetValueOrThrow("Cached result could not be refreshed");
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }
}