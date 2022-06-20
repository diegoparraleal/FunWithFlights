using FunWithFlights.Core.Misc;
using Newtonsoft.Json;

namespace FunWithFlights.Domain.Equipments;

[JsonConverter(typeof(ToStringJsonConverter))]
public record EquipmentCode
{
    private string Code { get; }

    public EquipmentCode(string code)
    {
        ValidateCode(code);
        Code = code.ToUpper();
    }

    private static void ValidateCode(string code)
    {
        if (code == null) throw new ArgumentException("Equipment code should not be null");
    }

    public static implicit operator EquipmentCode(string code) => new (code);
    public static explicit operator string(EquipmentCode equipmentCode) => equipmentCode.Code;
    public override string ToString() => Code;
}