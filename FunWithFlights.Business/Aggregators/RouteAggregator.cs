// using CSharpFunctionalExtensions;
// using FunWithFlights.Core.Aggregators;
// using FunWithFlights.Core.Extensions;
// using FunWithFlights.Domain.ExternalProviders;
// using FunWithFlights.Domain.Routes;
// using FunWithFlights.Infrastructure.Contracts.ExternalProviders;
// using FunWithFlights.Infrastructure.Contracts.Repositories;
//
// namespace FunWithFlights.Business.Aggregators;
// using RouteCollection = IReadOnlyDictionary<RouteKey, Route>;
//
// public interface IRouteAggregator: IAggregator<IReadOnlyCollection<Route>> {}
//
// public class RouteAggregator: IRouteAggregator
// {
//     private readonly IExternalRouteProviderFactory _externalRouteProviderFactory;
//     private readonly IExternalProviderRepository _externalProviderRepository;
//     private readonly IAirlineRepository _airlineRepository;
//     private readonly IAirportRepository _airportRepository;
//     private readonly IEquipmentRepository _equipmentRepository;
//
//     public RouteAggregator(
//         IExternalRouteProviderFactory externalRouteProviderFactory,
//         IExternalProviderRepository externalProviderRepository, 
//         IAirlineRepository airlineRepository,
//         IAirportRepository airportRepository,
//         IEquipmentRepository equipmentRepository
//     )
//     {
//         _externalProviderRepository = externalProviderRepository;
//         _airlineRepository = airlineRepository;
//         _airportRepository = airportRepository;
//         _equipmentRepository = equipmentRepository;
//         _externalRouteProviderFactory = externalRouteProviderFactory;
//     }
//     
//     public async Task<IReadOnlyCollection<Route>> ExecuteAsync()
//     {
//         var externalProviders = await _externalProviderRepository.GetAllAsync();
//         var routeProviders = externalProviders.Select(x => _externalRouteProviderFactory.Get(x));
//         var routes = await FetchRoutes(routeProviders);
//         return routes.AsReadOnly();
//     }
//
//     private async Task<IEnumerable<Route>> FetchRoutes(IEnumerable<IExternalRouteProvider> routeProviders)
//     {
//         var externalRoutes = await routeProviders.SelectManyAsync(x => x.GetAllRoutesAsync());
//         var routes = await externalRoutes
//             .GroupBy(RouteKey.FromExternalRoute)
//             .SelectAsync(BuildRoute);
//             
//         return routes;
//     }
//
//     private async Task<Route> BuildRoute(IGrouping<RouteKey, ExternalRoute> grouping)
//     {
//         var externalRoute = grouping.First(); // Always take first route if there is a collision
//         return new Route(
//             await _airlineRepository.GetByKeyAsync(externalRoute.Airline).GetValueOrThrow($"Unknown airline {externalRoute.Airline}"),
//             await _airportRepository.GetByKeyAsync(externalRoute.SourceAirport).GetValueOrThrow($"Unknown airport {externalRoute.SourceAirport}"),
//             await _airportRepository.GetByKeyAsync(externalRoute.DestinationAirport).GetValueOrThrow($"Unknown airport {externalRoute.DestinationAirport}"),
//             externalRoute.CodeShare,
//             externalRoute.Stops,
//             await _equipmentRepository.GetByKeyAsync(externalRoute.Equipment)
//         );
//     }
// }