using FunWithFlights.Infrastructure.Contracts.Repositories;
using FunWithFlights.Infrastructure.InMemory.EmbeddedRepositories;

// ReSharper disable StringLiteralTypo

namespace FunWithFlights.Infrastructure.Tests;

public class AirportRepositoryTests
{
    private IAirportRepository _airportRepository = null!;
    private const string AirportCsv = "FunWithFlights.Infrastructure.Tests.Csv.Airports.csv";

    [SetUp]
    public void Setup()
    {
        _airportRepository = new AirportEmbeddedRepository(AirportCsv);
    }

    [Test]
    public async Task Should_load_all_airports()
    {
        // Act
        var results = await _airportRepository.GetAllAsync();

        // Assert
        Assert.That(results.Count, Is.EqualTo(7));
        Assert.That(results.First().Name, Is.EqualTo("Goroka Airport"));
    }
    
    [Test]
    public async Task Should_get_airport_by_key()
    {
        // Act
        var results = await _airportRepository.GetByKeyAsync("UAK");

        // Assert
        Assert.That(results.HasValue, Is.True);
        Assert.That(results.GetValueOrDefault().Name, Is.EqualTo("Narsarsuaq Airport"));
    }
}