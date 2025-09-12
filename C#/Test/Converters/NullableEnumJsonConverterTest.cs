using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Utils.test.Converters;

using Utils.Converters;

[ExcludeFromCodeCoverage]
public class NullableEnumJsonConverterTest
{
    public enum TestEnum
    {
        Value1 = 1,
        Value2 = 2,
    }

    public class TestModelWithNullableEnum
    {
        [JsonConverter(typeof(NullableEnumJsonConverter<TestEnum?>))]
        public TestEnum? TestEnum { get; set; }
    }

    [Theory]
    [InlineData("1", TestEnum.Value1)]
    [InlineData("2", TestEnum.Value2)]
    public void Read_WhenJsonTokenTypeIsStringAndParsable_ShouldReturnEnum(string input, TestEnum expected)
    {
        var options = GetJsonSerializerOptions(typeof(NullableEnumJsonConverter<TestEnum?>));
        var json = $"{{\"testEnum\": \"{input}\"}}";
        var result = JsonSerializer.Deserialize<TestModelWithNullableEnum>(json, options);
        Assert.Equal(expected, result!.TestEnum);
    }

    [Theory]
    [InlineData("-1")]
    [InlineData("3")]
    public void Read_WhenJsonTokenTypeIsStringAndEnumValueNotDefined_ShouldThrowException(string input)
    {
        var options = GetJsonSerializerOptions(typeof(NullableEnumJsonConverter<TestEnum?>));
        var json = $"{{\"testEnum\": \"{input}\"}}";
        Assert.Throws<Exception>(() => JsonSerializer.Deserialize<TestModelWithNullableEnum>(json, options));
    }

    [Fact]
    public void Read_WhenJsonTokenTypeIsStringAndNotParsable_ShouldThrowException()
    {
        var options = GetJsonSerializerOptions(typeof(NullableEnumJsonConverter<TestEnum?>));
        var json = $"{{\"testEnum\": \"test\"}}";
        Assert.Throws<Exception>(() => JsonSerializer.Deserialize<TestModelWithNullableEnum>(json, options));
    }

    [Fact]
    public void Read_WhenJsonTokenTypeIsStringAndWhiteSpace_ShouldReturnNull()
    {
        var options = GetJsonSerializerOptions(typeof(NullableEnumJsonConverter<TestEnum?>));
        var json = $"{{\"testEnum\": \"\"}}";
        var result = JsonSerializer.Deserialize<TestModelWithNullableEnum>(json, options);
        Assert.Null(result!.TestEnum);
    }

    [Theory]
    [InlineData(1, TestEnum.Value1)]
    [InlineData(2, TestEnum.Value2)]
    public void Read_WhenJsonTokenTypeIsNumberAndEnumValueDefined_ShouldReturnEnum(int input, TestEnum expected)
    {
        var options = GetJsonSerializerOptions(typeof(NullableEnumJsonConverter<TestEnum?>));
        var json = $"{{\"testEnum\": {input}}}";
        var result = JsonSerializer.Deserialize<TestModelWithNullableEnum>(json, options);
        Assert.Equal(expected, result!.TestEnum);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(3)]
    public void Read_WhenJsonTokenTypeIsNumberAndEnumValueNotDefined_ShouldThrowException(int input)
    {
        var options = GetJsonSerializerOptions(typeof(NullableEnumJsonConverter<TestEnum?>));
        var json = $"{{\"testEnum\": {input}}}";
        Assert.Throws<Exception>(() => JsonSerializer.Deserialize<TestModelWithNullableEnum>(json, options));
    }

    [Fact]
    public void Read_WhenJsonTokenTypeIsNotSupported_ShouldThrowException()
    {
        var options = GetJsonSerializerOptions(typeof(NullableEnumJsonConverter<TestEnum?>));
        var json = $"{{\"testEnum\": true}}";
        Assert.Throws<Exception>(() => JsonSerializer.Deserialize<TestModelWithNullableEnum>(json, options));
    }

    [Fact]
    public void Constructor_WhenTypeIsNullableEnum_ShouldSucceed()
    {
        Assert.Throws<InvalidOperationException>(() => new NullableEnumJsonConverter<DayOfWeek>());
    }

    [Fact]
    public void Constructor_WhenTypeIsNotNullableEnum_ShouldThrowInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => new NullableEnumJsonConverter<int?>());
    }

    [Fact]
    public void Write_ValueIsNull_ShouldReturnJson()
    {
        var value = new TestModelWithNullableEnum()
        {
            TestEnum = null
        };
        var options = GetJsonSerializerOptions(typeof(NullableEnumJsonConverter<TestEnum?>));
        var json = JsonSerializer.Serialize(value, options);
        Assert.Equal("{\"testEnum\":null}", json);
    }

    [Fact]
    public void Write_ValueIsNotNull_ShouldReturnJson()
    {
        var value = new TestModelWithNullableEnum
        {
            TestEnum = TestEnum.Value1
        };
        var options = GetJsonSerializerOptions(typeof(NullableEnumJsonConverter<TestEnum?>));
        var json = JsonSerializer.Serialize(value, options);
        Assert.Equal("{\"testEnum\":\"Value1\"}", json);
    }

    private static JsonSerializerOptions GetJsonSerializerOptions(Type converterType)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        if (Activator.CreateInstance(converterType) is JsonConverter converter)
        {
            options.Converters.Add(converter);
        }
        return options;
    }
}