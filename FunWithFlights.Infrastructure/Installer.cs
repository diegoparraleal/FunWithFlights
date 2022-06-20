using FunWithFlights.Core.DependencyInjection;
using FunWithFlights.Infrastructure.Cache;
using FunWithFlights.Infrastructure.Contracts.Cache;
using FunWithFlights.Infrastructure.Contracts.ExternalProviders;
using FunWithFlights.Infrastructure.Contracts.Repositories;
using FunWithFlights.Infrastructure.ExternalProviders;
using FunWithFlights.Infrastructure.InMemory.EmbeddedRepositories;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace FunWithFlights.Infrastructure;

[UsedImplicitly]
public class Installer: IInstaller
{
    private const string AirlineCsv = "FunWithFlights.Infrastructure.InMemory.Csv.Airlines.csv";
    private const string AirportsCsv = "FunWithFlights.Infrastructure.InMemory.Csv.Airports.csv";
    private const string AirportDistancesCsv = "FunWithFlights.Infrastructure.InMemory.Csv.AirportDistances.csv";
    private const string AircraftsCsv = "FunWithFlights.Infrastructure.InMemory.Csv.Aircrafts.csv";
    private const string ExternalProvidersCsv = "FunWithFlights.Infrastructure.InMemory.Csv.ExternalProviders.csv";
    
    public void RegisterDependencies(IServiceCollection services)
    {
        services.AddSingleton<IExternalRouteProviderFactory, ExternalRouteProviderFactory>();
        services.AddSingleton<ICacheProvider, CacheProvider>();

        // NOTE: Here we can switch implementations based on ENV flags. e.g. using TableStorage
        services.AddSingleton<IAirlineRepository>(new AirlineEmbeddedRepository(AirlineCsv) );
        services.AddSingleton<IAirportRepository>(new AirportEmbeddedRepository(AirportsCsv) );
        services.AddSingleton<IAirportDistancesRepository>(new AirportDistancesEmbeddedRepository(AirportDistancesCsv) );
        services.AddSingleton<IEquipmentRepository>(new EquipmentEmbeddedRepository(AircraftsCsv) );
        services.AddSingleton<IExternalProviderRepository>(new ExternalProviderEmbeddedRepository(ExternalProvidersCsv) );
    }
}