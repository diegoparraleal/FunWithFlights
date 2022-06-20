using FunWithFlights.Core.Entities;
using Newtonsoft.Json;

namespace FunWithFlights.Domain.Airlines;

public record Airline(AirlineCode Code, string Name, string Iata, string Country, bool Active): IEntity<AirlineCode>
{
    [JsonIgnore]
    public AirlineCode Key => Code;
}