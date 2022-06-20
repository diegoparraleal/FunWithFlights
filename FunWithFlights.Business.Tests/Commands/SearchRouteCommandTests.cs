using FunWithFlights.Business.Aggregators;
using FunWithFlights.Business.Commands;
using FunWithFlights.Domain;
using FunWithFlights.Domain.Airlines;
using FunWithFlights.Domain.Airports;
using FunWithFlights.Domain.Equipments;
using FunWithFlights.Domain.ExternalProviders;
using FunWithFlights.Infrastructure.Contracts.Repositories;
using NSubstitute;

namespace FunWithFlights.Business.Tests;

public class SearchRouteCommandTests
{
    private ICachedExternalRouteAggregator _cachedExternalRouteAggregator = null!;
    private IAirlineRepository _airlineRepository = null!;
    private IAirportRepository _airportRepository = null!;
    private IEquipmentRepository _equipmentRepository = null!;
    private IAirportDistancesRepository _airportDistancesRepository = null!;

    private ISearchRouteCommand _searchRouteCommand = null!;

    private static readonly Airline Airline = new ("XX", "Norwegian Air International", "ZK", "Norway", true);
    private static readonly Airport Airport = new ("XXX", "Goroka Airport", "Goroka", "Papua New Guinea", -6.081689834590001m,145.391998291m,5282m);
    private static readonly Equipment Equipment = new ("XXX", "Airbus A300 pax", 200, "European consortium");
    private static readonly ExternalRoute ExternalRoute = new ("XX", "XX", "XX", "", 1, "XXX");

    [SetUp]
    public void Setup()
    {
        _cachedExternalRouteAggregator = Substitute.For<ICachedExternalRouteAggregator>();
        _airlineRepository = Substitute.For<IAirlineRepository>();
        _airportRepository = Substitute.For<IAirportRepository>();
        _equipmentRepository = Substitute.For<IEquipmentRepository>();
        _airportDistancesRepository = Substitute.For<IAirportDistancesRepository>();

        _searchRouteCommand = new SearchRouteCommand(_cachedExternalRouteAggregator, _airlineRepository, _airportRepository, _equipmentRepository, _airportDistancesRepository);
        _airlineRepository.GetByKeyAsync(default!).ReturnsForAnyArgs(x => Airline with { Code = x?.Args()[0].ToString() ?? Airline.Code });
        _airportRepository.GetByKeyAsync(default!).ReturnsForAnyArgs(x => Airport  with { Code = x?.Args()[0].ToString() ?? Airport.Code });
        _equipmentRepository.GetByKeyAsync(default!).ReturnsForAnyArgs(x => Equipment  with { Code = x?.Args()[0].ToString() ?? Equipment.Code });
        _cachedExternalRouteAggregator.GetRouteByKey(default!)
            .ReturnsForAnyArgs(x =>
            {
                var key = x?.Arg<RouteKey>();
                if (key == null) return ExternalRoute;
                return ExternalRoute with { Airline = key.Airline, SourceAirport = key.Source, DestinationAirport = key.Destination};
            });
        _cachedExternalRouteAggregator.GetRoutesFrom("A01").Returns(new List<RouteKey>
        {
            new ("A01", "A02", "XX"),
            new ("A01", "A03", "XX"),
            new ("A01", "A04", "XX"),
        });
        _cachedExternalRouteAggregator.GetRoutesFrom("A02").Returns(new List<RouteKey>
        {
            new ("A02", "A01", "XX"),
            new ("A02", "A03", "XX"),
            new ("A02", "A04", "XX")
        });
        _cachedExternalRouteAggregator.GetRoutesFrom("A03").Returns(new List<RouteKey>
        {
            new ("A03", "A01", "XX"),
            new ("A03", "A02", "XX"),
            new ("A03", "A04", "XX")
        });
    }
    
    [Test]
    public async Task Can_search_direct_and_indirect_routes()
    {
        // Arrange
        var parameters = new SearchRouteParameters("A01", "A04");
        
        // Act
        var routes = await _searchRouteCommand.ExecuteAsync(parameters);

        // Assert
        Assert.That(routes.Results.Count(), Is.EqualTo(3));
        Assert.That(routes.Results.Count(x => x.NumStops == 1), Is.EqualTo(1));
        Assert.That(routes.Results.Count(x => x.NumStops == 2), Is.EqualTo(2));
        Assert.That(routes.Results.All(x => x.Routes.First().Source.Code == "A01"), Is.True);
        Assert.That(routes.Results.All(x => x.Routes.Last().Destination.Code == "A04"), Is.True);
    }
}