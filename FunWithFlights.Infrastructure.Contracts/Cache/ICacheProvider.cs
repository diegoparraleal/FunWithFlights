namespace FunWithFlights.Infrastructure.Contracts.Cache;

public enum CacheKey
{
    Routes
}

public interface ICacheProvider
{
    Task<T> GetAsync<T>(CacheKey key, Func<Task<T>> callback, TimeSpan toExpire = default);
}