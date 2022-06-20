using FunWithFlights.Infrastructure.Contracts.Repositories;
using FunWithFlights.Infrastructure.InMemory.EmbeddedRepositories;

// ReSharper disable StringLiteralTypo

namespace FunWithFlights.Infrastructure.Tests;

public class AirlineRepositoryTests
{
    private IAirlineRepository _airlineRepository = null!;
    private const string AirlineCsv = "FunWithFlights.Infrastructure.Tests.Csv.Airlines.csv";

    [SetUp]
    public void Setup()
    {
        _airlineRepository = new AirlineEmbeddedRepository(AirlineCsv);
    }

    [Test]
    public async Task Should_load_all_airlines()
    {
        // Act
        var results = await _airlineRepository.GetAllAsync();

        // Assert
        Assert.That(results.Count, Is.EqualTo(10));
        Assert.That(results.First().Name, Is.EqualTo("ABSA Cargo Airline"));
    }
    
    [Test]
    public async Task Should_get_airline_by_key()
    {
        // Act
        var results = await _airlineRepository.GetByKeyAsync("8U");

        // Assert
        Assert.That(results.HasValue, Is.True);
        Assert.That(results.GetValueOrDefault().Name, Is.EqualTo("Afriqiyah Airways"));
    }
}