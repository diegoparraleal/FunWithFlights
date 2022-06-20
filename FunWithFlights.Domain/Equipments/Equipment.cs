using FunWithFlights.Core.Entities;
using Newtonsoft.Json;

namespace FunWithFlights.Domain.Equipments;

public record Equipment(EquipmentCode Code, string Name, int Capacity, string Country): IEntity<EquipmentCode>
{
    [JsonIgnore]
    public EquipmentCode Key => Code;
}