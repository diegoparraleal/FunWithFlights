using CSharpFunctionalExtensions;
using FunWithFlights.Core.Cache;
using FunWithFlights.Core.Time;
using FunWithFlights.Domain;
using FunWithFlights.Domain.Airports;
using FunWithFlights.Domain.ExternalProviders;

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
    private const int MinutesToExpire = 60;
    private readonly CachedResult<State> _cachedResult;

    public CachedExternalRouteAggregator(IExternalRouteAggregator externalRouteAggregator, IClock clock)
    {
        _externalRouteAggregator = externalRouteAggregator;
        _cachedResult = new CachedResult<State>(
            TimeSpan.FromMinutes(MinutesToExpire),
            LoadAsync,
            clock);
    }

    public async Task<IReadOnlyCollection<ExternalRoute>> ExecuteAsync() => (await _cachedResult.GetAsync()).Routes;

    public async Task<IEnumerable<RouteKey>> GetRoutesFrom(AirportCode airportCode)
    {
        var lookup = (await _cachedResult.GetAsync()).LookupPerSourceAirport;
        return lookup.TryFind(airportCode).GetValueOrDefault(Array.Empty<RouteKey>());
    }

    public async Task<Maybe<ExternalRoute>> GetRouteByKey(RouteKey key)
    {
        var lookup = (await _cachedResult.GetAsync()).LookupPerRouteKey;
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
}