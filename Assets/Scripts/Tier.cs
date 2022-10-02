using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[JsonConverter(typeof(StringEnumConverter))]
public enum Tier
{
    X,
    S,
    A,
    B,
    C,
    D
}