using FunWithFlights.Core.Extensions;
using FunWithFlights.Domain.Equipments;
using FunWithFlights.Infrastructure.Contracts.Repositories;

namespace FunWithFlights.Infrastructure.InMemory.EmbeddedRepositories;

public class EquipmentEmbeddedRepository: CsvEmbeddedRepository<EquipmentCode, Equipment>, IEquipmentRepository
{
    public EquipmentEmbeddedRepository(string file) : base(file)
    {
    }

    protected override Equipment MapRecord(Dictionary<string, string> record)
        => new (
            record.MaybeGet("Code").ValueOrDefault(),
            record.MaybeGet("Name").ValueOrDefault(),
            record.MaybeGet("Capacity")
                .Then(x => x.MaybeParseAsInt().ValueOrDefault())
                .ValueOrDefault(),
            record.MaybeGet("Country").ValueOrDefault()
        );
}