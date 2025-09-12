using System.Text.Json;
using System.Text.Json.Serialization;

namespace Utils.Converters;

/// <summary>
/// NullableEnumJsonConverter
/// </summary>
/// <remarks>
/// T is needed to be nullable enum type.
/// </remarks>
/// <typeparam name="T"></typeparam>
public class NullableEnumJsonConverter<T> : JsonConverter<T>
{
    private readonly Type _underlyingType = null!;

    public NullableEnumJsonConverter()
    {
        _underlyingType = Nullable.GetUnderlyingType(typeof(T))!;
        if (_underlyingType == null || !_underlyingType.IsEnum)
        {
            throw new InvalidOperationException("NullableEnumJsonConverter can only be used with nullable enum types.");
        }
    }

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        int intValue;
        switch (reader.TokenType)
        {
            case JsonTokenType.String:
                {
                    var inputString = reader.GetString();
                    if (string.IsNullOrWhiteSpace(inputString))
                    {
                        return default;
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

            default:
                throw new Exception($"Unexpected token parsing enum. Expected String or Number, got {reader.TokenType}.");
        }

        if (!Enum.IsDefined(_underlyingType, intValue))
        {
            throw new Exception($"{intValue} is not defined in {typeof(T).Name}");
        }
        return (T)Enum.ToObject(_underlyingType, intValue);
    }

    public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
