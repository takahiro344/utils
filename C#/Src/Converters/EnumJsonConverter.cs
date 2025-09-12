using System.Text.Json;
using System.Text.Json.Serialization;

namespace Utils.Converters;

/// <summary>
/// EnumJsonConverter
/// </summary>
/// <remarks>
/// T is needed to be not nullable enum type.
/// </remarks>
/// <typeparam name="T"></typeparam>
public class EnumJsonConverter<T> : JsonConverter<T> where T : struct, Enum
{
    private readonly Type _underlyingType = typeof(T);

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        int intValue;
        switch (reader.TokenType)
        {
            case JsonTokenType.String:
                {
                    var inputString = reader.GetString();
                    if (string.IsNullOrWhiteSpace(inputString))
                    {
                        throw new Exception("Input string is null or white space.");
                    }

                    if (int.TryParse(inputString, out intValue))
                    {
                        break;
                    }
                    throw new Exception($"Failed to parse '{inputString}' to an integer.");
                }

            case JsonTokenType.Number:
                {
                    intValue = reader.GetInt32();
                    break;
                }

            case JsonTokenType.Null:
                throw new Exception("Cannot convert null value to enum.");

            default:
                throw new Exception($"Unexpected token parsing enum. Expected String or Number, got {reader.TokenType}.");
        }

        if (!Enum.IsDefined(_underlyingType, intValue))
        {
            throw new Exception($"{intValue} is not defined in {typeof(T).Name}");
        }
        return (T)Enum.ToObject(_underlyingType, intValue);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
