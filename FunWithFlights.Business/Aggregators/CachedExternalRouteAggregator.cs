using CSharpFunctionalExtensions;
using FunWithFlights.Domain;
using FunWithFlights.Domain.Airports;
using FunWithFlights.Domain.ExternalProviders;
using FunWithFlights.Infrastructure.Contracts.Cache;

namespace FunWithFlights.Business.Aggregators;

internal record State(
    IReadOnlyCollection<ExternalRoute> Routes,
    IReadOnlyDictionary<RouteKey, ExternalRoute> LookupPerRouteKey,
    IReadOnlyDictionary<AirportCode, IEnumerable<RouteKey>> LookupPerSourceAirport);

public interface ICachedExternalRouteAggregator : IExternalRouteAggregator
{
    Task<IEnumerable<RouteKey>> GetRoutesFrom(AirportCode airportCode);
    Task<Maybe<ExternalRoute>> GetRouteByKey(RouteKey key);
}

public class CachedExternalRouteAggregator: ICachedExternalRouteAggregator
{
    private readonly IExternalRouteAggregator _externalRouteAggregator;
    private readonly ICacheProvider _cacheProvider;
    private const int MinutesToExpire = 60;

    public CachedExternalRouteAggregator(IExternalRouteAggregator externalRouteAggregator, ICacheProvider cacheProvider)
    {
        _externalRouteAggregator = externalRouteAggregator;
        _cacheProvider = cacheProvider;
    }

    public async Task<IReadOnlyCollection<ExternalRoute>> ExecuteAsync() => (await GetStateAsync()).Routes;

    public async Task<IEnumerable<RouteKey>> GetRoutesFrom(AirportCode airportCode)
    {
        var lookup = (await GetStateAsync()).LookupPerSourceAirport;
        return lookup.TryFind(airportCode).GetValueOrDefault(Array.Empty<RouteKey>());
    }

    public async Task<Maybe<ExternalRoute>> GetRouteByKey(RouteKey key)
    {
        var lookup = (await GetStateAsync()).LookupPerRouteKey;
        return lookup.TryFind(key);    
    }

    private async Task<State> LoadAsync()
    {
        var results = await _externalRouteAggregator.ExecuteAsync();
        var sourceAirportLookup = results
            .GroupBy(x => (AirportCode)x.SourceAirport)
            .ToDictionary(k => k.Key, v=> v.Select(RouteKey.FromExternalRoute));
        return new State(results, results.ToDictionary(RouteKey.FromExternalRoute), sourceAirportLookup);
    }

    private async Task<State> GetStateAsync() =>
        await _cacheProvider.GetAsync(CacheKey.Routes, LoadAsync, TimeSpan.FromMinutes(MinutesToExpire));
}