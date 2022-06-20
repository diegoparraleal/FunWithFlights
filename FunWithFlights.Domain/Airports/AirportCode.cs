using FunWithFlights.Core.Misc;
using Newtonsoft.Json;

namespace FunWithFlights.Domain.Airports;

[JsonConverter(typeof(ToStringJsonConverter))]
public record AirportCode
{
    private string Code { get; }

    public AirportCode(string code)
    {
        ValidateCode(code);
        Code = code.ToUpper();
    }

    private static void ValidateCode(string code)
    {
        if (code == null) throw new ArgumentException("Airport code should not be null");
        if (code.Length != 3) throw new ArgumentException("Airport code should only have 3 letters");
    }

    public static implicit operator AirportCode(string code) => new (code);
    public static implicit operator string(AirportCode airportCode) => airportCode.Code;
    public override string ToString() => Code;
}