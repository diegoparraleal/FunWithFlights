using FunWithFlights.Core.Aggregators;
using FunWithFlights.Core.Extensions;
using FunWithFlights.Domain;
using FunWithFlights.Domain.ExternalProviders;
using FunWithFlights.Infrastructure.Contracts.ExternalProviders;
using FunWithFlights.Infrastructure.Contracts.Repositories;

namespace FunWithFlights.Business.Aggregators;

public interface IExternalRouteAggregator: IAggregator<IReadOnlyCollection<ExternalRoute>> {}

public class ExternalRouteAggregator: IExternalRouteAggregator
{
    private readonly IExternalRouteProviderFactory _externalRouteProviderFactory;
    private readonly IExternalProviderRepository _externalProviderRepository;

    public ExternalRouteAggregator(
        IExternalRouteProviderFactory externalRouteProviderFactory,
        IExternalProviderRepository externalProviderRepository
    )
    {
        _externalProviderRepository = externalProviderRepository;
        _externalRouteProviderFactory = externalRouteProviderFactory;
    }
    
    public async Task<IReadOnlyCollection<ExternalRoute>> ExecuteAsync()
    {
        var externalProviders = await _externalProviderRepository.GetAllAsync();
        var routeProviders = externalProviders.Select(x => _externalRouteProviderFactory.Get(x));
        var routes = await FetchRoutes(routeProviders);
        return routes.AsReadOnly();
    }

    private async Task<IEnumerable<ExternalRoute>> FetchRoutes(IEnumerable<IExternalRouteProvider> routeProviders)
    {
        var externalRoutes = await routeProviders.SelectManyAsync(x => x.GetAllRoutesAsync());
        return externalRoutes
            .GroupBy(RouteKey.FromExternalRoute)
            .Select(BuildRoute);
    }

    private ExternalRoute BuildRoute(IGrouping<RouteKey, ExternalRoute> grouping) => grouping.First(); // Always take first route if there is a collision
}