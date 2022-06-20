using System.Collections.Concurrent;
using FunWithFlights.Core.Cache;
using FunWithFlights.Core.Extensions;
using FunWithFlights.Core.Time;
using FunWithFlights.Infrastructure.Contracts.Cache;

namespace FunWithFlights.Infrastructure.Cache;

public class CacheProvider: ICacheProvider
{
    private readonly IClock _clock;
    private ConcurrentDictionary<CacheKey, ICachedResult> _store = new();

    public CacheProvider(IClock clock)
    {
        _clock = clock;
    }
    
    public async Task<T> GetAsync<T>(CacheKey key, Func<Task<T>> callback, TimeSpan toExpire = default)
    {
        if (_store.TryGetValue(key, out var cacheResult))
        {
            if (cacheResult.MaybeCast<ICachedResult<T>>().TryGet(out var typedCacheResult))
            {
                return await typedCacheResult.GetAsync();
            }
        }

        var newCachedResult = new CachedResult<T>(toExpire, callback, _clock);
        _store.AddOrUpdate(key, newCachedResult, (_, x) => x);
        return await newCachedResult.GetAsync();
    }
}