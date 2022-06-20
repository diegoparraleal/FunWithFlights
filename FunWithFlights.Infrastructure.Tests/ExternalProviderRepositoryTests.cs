using FunWithFlights.Infrastructure.Contracts.Repositories;
using FunWithFlights.Infrastructure.InMemory.EmbeddedRepositories;

// ReSharper disable StringLiteralTypo

namespace FunWithFlights.Infrastructure.Tests;

public class ExternalProviderRepositoryTests
{
    private IExternalProviderRepository _externalProviderRepository = null!;
    private const string ExternalProviderCsv = "FunWithFlights.Infrastructure.Tests.Csv.ExternalProviders.csv";

    [SetUp]
    public void Setup()
    {
        _externalProviderRepository = new ExternalProviderEmbeddedRepository(ExternalProviderCsv);
    }

    [Test]
    public async Task Should_load_all_external_providers_by_source()
    {
        // Act
        var results = await _externalProviderRepository.GetAllBySourceAsync("B");

        // Assert
        Assert.That(results.Count, Is.EqualTo(2));
        Assert.That(results.First().Source, Is.EqualTo("B"));
        Assert.That(results.First().Code, Is.EqualTo("B1"));
    }
    
    [Test]
    public async Task Should_get_equipment_by_key()
    {
        // Act
        var results = await _externalProviderRepository.GetByKeyAsync("A2");

        // Assert
        Assert.That(results.HasValue, Is.True);
        Assert.That(results.GetValueOrDefault().Url.AbsoluteUri, Is.EqualTo("http://ec2-3-70-19-16.eu-central-1.compute.amazonaws.com:8000/provider/flights2"));
    }
}