using FunWithFlights.Infrastructure.Contracts.Repositories;
using FunWithFlights.Infrastructure.InMemory.EmbeddedRepositories;

// ReSharper disable StringLiteralTypo

namespace FunWithFlights.Infrastructure.Tests;

public class EquipmentRepositoryTests
{
    private IEquipmentRepository _equipmentRepository = null!;
    private const string EquipmentCsv = "FunWithFlights.Infrastructure.Tests.Csv.Aircrafts.csv";

    [SetUp]
    public void Setup()
    {
        _equipmentRepository = new EquipmentEmbeddedRepository(EquipmentCsv);
    }

    [Test]
    public async Task Should_load_all_equipments()
    {
        // Act
        var results = await _equipmentRepository.GetAllAsync();

        // Assert
        Assert.That(results.Count, Is.EqualTo(10));
        Assert.That(results.First().Name, Is.EqualTo("Aerospatiale/Alenia ATR 42-300 / 320"));
    }
    
    [Test]
    public async Task Should_get_equipment_by_key()
    {
        // Act
        var results = await _equipmentRepository.GetByKeyAsync("31X");

        // Assert
        Assert.That(results.HasValue, Is.True);
        Assert.That(results.GetValueOrDefault().Name, Is.EqualTo("Airbus A310-200 Freighter"));
    }
}