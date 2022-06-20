using FunWithFlights.Business.Aggregators;
using FunWithFlights.Business.Commands;
using FunWithFlights.Business.Queries;
using FunWithFlights.Core.DependencyInjection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace FunWithFlights.Business;

[UsedImplicitly]
public class Installer: IInstaller
{
    public void RegisterDependencies(IServiceCollection services)
    {
        services.AddSingleton<IAirlineQueries, AirlineQueries>();
        services.AddSingleton<IAirportQueries, AirportQueries>();
        services.AddSingleton<IEquipmentQueries, EquipmentQueries>();
        services.AddSingleton<IExternalProviderQueries, ExternalProviderQueries>();
        services.AddSingleton<IExternalRouteAggregator, ExternalRouteAggregator>();
        services.AddSingleton<ICachedExternalRouteAggregator, CachedExternalRouteAggregator>();
        services.AddSingleton<ISearchRouteCommand, SearchRouteCommand>();
    }
}