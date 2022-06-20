using FunWithFlights.Business.Aggregators;
using FunWithFlights.Core.Extensions;
using FunWithFlights.Domain;
using FunWithFlights.Domain.ExternalProviders;
using FunWithFlights.Infrastructure.Contracts.ExternalProviders;
using FunWithFlights.Infrastructure.Contracts.Repositories;
using NSubstitute;

namespace FunWithFlights.Business.Tests.Aggregators;

public class RouteAggregatorTests
{
    private IExternalRouteProviderFactory _externalRouteProviderFactory = null!;
    private IExternalRouteProvider _externalRouteProvider1 = null!;
    private IExternalRouteProvider _externalRouteProvider2 = null!;
    private IExternalProviderRepository _externalProviderRepository = null!;
    
    private IExternalRouteAggregator _externalRouteAggregator = null!;

    private static readonly ExternalProvider Provider1 = new ("T1", "Test1", new Uri("http://test1"));
    private static readonly ExternalProvider Provider2 = new ("T2", "Test2", new Uri("http://test2"));
    private static readonly ExternalRoute ExternalRoute = new ("XX", "XX", "XX", "", 1, "XXX");

    [SetUp]
    public void Setup()
    {
        _externalRouteProviderFactory = Substitute.For<IExternalRouteProviderFactory>();
        _externalProviderRepository = Substitute.For<IExternalProviderRepository>();
        _externalRouteProvider1 = Substitute.For<IExternalRouteProvider>();
        _externalRouteProvider2 = Substitute.For<IExternalRouteProvider>();

        _externalRouteAggregator = new ExternalRouteAggregator(_externalRouteProviderFactory, _externalProviderRepository);
        _externalProviderRepository.GetAllAsync().ReturnsForAnyArgs(new [] { Provider1, Provider2});
        _externalRouteProviderFactory.Get(Provider1).Returns(_externalRouteProvider1);
        _externalRouteProviderFactory.Get(Provider2).Returns(_externalRouteProvider2);
    }
    
    [Test]
    public async Task Should_merge_routes()
    {
        // Arrange
        _externalRouteProvider1.GetAllRoutesAsync().Returns(Enumerable.Range(1, 10).Select(BuildExternalRoute).AsReadOnly());
        _externalRouteProvider2.GetAllRoutesAsync().Returns(Enumerable.Range(11, 10).Select(BuildExternalRoute).AsReadOnly());

        // Act
        var routes = await _externalRouteAggregator.ExecuteAsync();

        // Assert
        Assert.That(routes.Count, Is.EqualTo(20));
    }
    
    [Test]
    public async Task Should_not_have_duplicates_when_there_are_collisions()
    {
        // Arrange
        _externalRouteProvider1.GetAllRoutesAsync().Returns(Enumerable.Range(1, 12).Select(BuildExternalRoute).AsReadOnly());
        _externalRouteProvider2.GetAllRoutesAsync().Returns(Enumerable.Range(8, 13).Select(BuildExternalRoute).AsReadOnly());

        // Act
        var routes = await _externalRouteAggregator.ExecuteAsync();

        // Assert
        Assert.That(routes.Count, Is.EqualTo(20));
        // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
        Assert.DoesNotThrow(() => routes.ToDictionary(RouteKey.FromExternalRoute));
    }

    private ExternalRoute BuildExternalRoute(int i) 
        => ExternalRoute with
        {
            Airline = $"A{i}",
            SourceAirport = $"S{i:D2}",
            DestinationAirport = $"D{i:D2}",
            Equipment = $"E{i:D2}"
        };
}