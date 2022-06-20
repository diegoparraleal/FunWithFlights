using FunWithFlights.Core.Misc;
using Newtonsoft.Json;

namespace FunWithFlights.Domain.CodeShares;

[JsonConverter(typeof(ToStringJsonConverter))]
public record CodeShare
{
    private string Code { get; }

    private CodeShare(string code)
    {
        ValidateCode(code);
        Code = code.ToUpper();
    }

    private static void ValidateCode(string code)
    {
        if (code == null) throw new ArgumentException("Code share should not be null");
        if (code != "" && code.Length != 1) throw new ArgumentException("Code share should only have 1 letter");
    }

    public static implicit operator CodeShare(string code) => new (code);
    public static explicit operator string(CodeShare codeShare) => codeShare.Code;
    public override string ToString() => Code;
}

