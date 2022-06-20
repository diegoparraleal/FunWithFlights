using FunWithFlights.Business.Aggregators;
using FunWithFlights.Core.Commands;
using FunWithFlights.Core.Extensions;
using FunWithFlights.Domain;
using FunWithFlights.Domain.Airports;
using FunWithFlights.Domain.ExternalProviders;
using FunWithFlights.Domain.Routes;
using FunWithFlights.Infrastructure.Contracts.Repositories;
using MaybeExtensions = FunWithFlights.Core.Extensions.MaybeExtensions;

namespace FunWithFlights.Business.Commands;

public record SearchRouteParameters(AirportCode Source, AirportCode Destination): ICommandParameters;
public record SearchRouteResult(IEnumerable<CompositeRoute> Results): ICommandResult;

internal record RouteWithDistance(List<RouteKey> RouteKeys, decimal Distance);

public interface ISearchRouteCommand: ICommand<SearchRouteParameters, SearchRouteResult> { }

public class SearchRouteCommand: ISearchRouteCommand
{
    private const int MaxStops = 2;
    private const int MaxResults = 25;
    private readonly ICachedExternalRouteAggregator _cachedExternalRouteAggregator;
    private readonly IAirlineRepository _airlineRepository;
    private readonly IAirportRepository _airportRepository;
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IAirportDistancesRepository _airportDistancesRepository;

    public SearchRouteCommand(
        ICachedExternalRouteAggregator cachedExternalRouteAggregator,
        IAirlineRepository airlineRepository,
        IAirportRepository airportRepository,
        IEquipmentRepository equipmentRepository,
        IAirportDistancesRepository airportDistancesRepository
    )
    {
        _cachedExternalRouteAggregator = cachedExternalRouteAggregator;
        _airlineRepository = airlineRepository;
        _airportRepository = airportRepository;
        _equipmentRepository = equipmentRepository;
        _airportDistancesRepository = airportDistancesRepository;
    }
    
    public async Task<SearchRouteResult> ExecuteAsync(SearchRouteParameters parameters)
    {
        var externalRoutes = await FindRoutesAsync(parameters.Source, parameters.Destination);
        var selectedRoutes = await FilterRoutesAsync(externalRoutes);
        return new SearchRouteResult(await ConvertToCompositeRoute(selectedRoutes, parameters));
    }

    private async Task<IEnumerable<List<RouteKey>>> FindRoutesAsync(AirportCode source, AirportCode destination, AirportCode? originalSource = default, int numSteps = 1, List<RouteKey>? stack = default)
    {
        stack ??= new List<RouteKey>();
        originalSource ??= source;

        var routes = await _cachedExternalRouteAggregator.GetRoutesFrom(source);
        var (found, others) = routes.Partition(x => x.Destination == destination);
        var result = found
            .Where(x => stack.Count < 1 || stack.First().Airline == x.Airline)
            .Select(x => stack.Append(x).ToList());
        
        if (numSteps >= MaxStops) return result;

        return result.Concat( 
                    await others
                    .Where(x => x.Destination != originalSource)
                    .SelectManyAsync(x => FindRoutesAsync(x.Destination, destination, originalSource, numSteps + 1, stack.Append(x).ToList()))
                );
    }
    
    private async Task<IEnumerable<RouteWithDistance>> FilterRoutesAsync(IEnumerable<List<RouteKey>> routes)
    {
        var routesWithDistance = await routes.SelectAsync(GetRouteWithDistance);
        return routesWithDistance.OrderBy(x => x.Distance).Take(MaxResults);
    }

    private async Task<RouteWithDistance> GetRouteWithDistance(List<RouteKey> routes)
    {
        var maybeDistances = await routes.SelectAsync(x => _airportDistancesRepository.GetByKeyAsync(AirportDistanceKey.FromRouteKey(x)));
        var distance = maybeDistances.Sum(s => s.Then(x => x.Distance).ValueOrDefault(9999));
        return new RouteWithDistance(routes, distance);
    }

    private async Task<IEnumerable<CompositeRoute>> ConvertToCompositeRoute(IEnumerable<RouteWithDistance> routes, SearchRouteParameters parameters)
    {
        var results = await routes.SelectAsync(x => MaybeExtensions.Execute(() => BuildCompositeRoute(x, parameters)));
        return results.SelectValid();
    }

    private async Task<CompositeRoute> BuildCompositeRoute(RouteWithDistance route, SearchRouteParameters parameters)
    {
        var externalRoutes = await GetExternalRoutes(route.RouteKeys);
        return new CompositeRoute(parameters.Source, parameters.Destination, route.RouteKeys.Count, route.Distance, await externalRoutes.SelectAsync(BuildRoute));
    }

    private async Task<Route> BuildRoute(ExternalRoute x)
    {
        var airline = await _airlineRepository.GetByKeyAsync(x.Airline);
        var source = await _airportRepository.GetByKeyAsync(x.SourceAirport);
        var destination = await _airportRepository.GetByKeyAsync(x.DestinationAirport);
        var equipment = await _equipmentRepository.GetByKeyAsync(x.Equipment);
        return new(
            airline.GetValueOrThrow($"Unknown airline {x.Airline}"), 
            source.GetValueOrThrow($"Unknown airport {x.SourceAirport}"),
            destination.GetValueOrThrow($"Unknown airport {x.SourceAirport}"), 
            x.CodeShare,
            x.Stops,
            equipment.GetValueOrDefault());
    }

    private async Task<IEnumerable<ExternalRoute>> GetExternalRoutes(IReadOnlyCollection<RouteKey> routeKeys)
    {
        var maybeExternalRoutes = await routeKeys.SelectAsync(_cachedExternalRouteAggregator.GetRouteByKey);
        return maybeExternalRoutes.SelectValid();
    }
}