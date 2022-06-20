using FunWithFlights.Core.Misc;
using Newtonsoft.Json;

namespace FunWithFlights.Domain.Airlines;

[JsonConverter(typeof(ToStringJsonConverter))]
public record AirlineCode
{
    private string Code { get; }

    public AirlineCode(string code)
    {
        ValidateCode(code);
        Code = code.ToUpper();
    }

    private static void ValidateCode(string code)
    {
        if (code == null) throw new ArgumentException("Airline code should not be null");
        if (code.Length < 2 || code.Length > 3) throw new ArgumentException("Airline code should only have 2 or 3 letters");
    }

    public static implicit operator AirlineCode(string code) => new (code);
    public static implicit operator string(AirlineCode airlineCode) => airlineCode.Code;
    public override string ToString() => Code;
}