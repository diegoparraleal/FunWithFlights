using FunWithFlights.Domain.Airports;
using FunWithFlights.Infrastructure.Contracts.Repositories;
using FunWithFlights.Infrastructure.InMemory.EmbeddedRepositories;

// ReSharper disable StringLiteralTypo

namespace FunWithFlights.Infrastructure.Tests;

public class AirportDistanceRepositoryTests
{
    private IAirportDistancesRepository _airportDistancesRepository = null!;
    private const string AirportDistancesCsv = "FunWithFlights.Infrastructure.Tests.Csv.AirportDistances.csv";

    [SetUp]
    public void Setup()
    {
        _airportDistancesRepository = new AirportDistancesEmbeddedRepository(AirportDistancesCsv);
    }

    [Test]
    public async Task Should_load_all_distances()
    {
        // Act
        var results = await _airportDistancesRepository.GetAllAsync();

        // Assert
        Assert.That(results.Count, Is.EqualTo(7));
        Assert.That(results.First().Distance, Is.EqualTo(100.089m));
    }
    
    [Test]
    public async Task Should_get_airport_by_key()
    {
        // Act
        var results = await _airportDistancesRepository.GetByKeyAsync(new AirportDistanceKey("ABC", "BCN"));

        // Assert
        Assert.That(results.HasValue, Is.True);
        Assert.That(results.GetValueOrDefault().Distance, Is.EqualTo(425.261m));
    }
}