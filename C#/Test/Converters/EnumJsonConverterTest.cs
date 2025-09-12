using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Utils.test.Converters;

using Utils.Converters;

[ExcludeFromCodeCoverage]
public class EnumJsonConverterTest
{
    public enum TestEnum
    {
        Value1 = 1,
        Value2 = 2,
    }

    public class TestModel
    {
        [JsonConverter(typeof(EnumJsonConverter<TestEnum>))]
        public TestEnum TestEnum { get; set; }
    }

    [Theory]
    [InlineData("1", TestEnum.Value1)]
    [InlineData("2", TestEnum.Value2)]
    public void Read_WhenJsonTokenTypeIsStringAndParsable_ShouldReturnEnum(string input, TestEnum expected)
    {
        var options = GetJsonSerializerOptions(typeof(EnumJsonConverter<TestEnum>));
        var json = $"{{\"testEnum\": \"{input}\"}}";
        var result = JsonSerializer.Deserialize<TestModel>(json, options);
        Assert.Equal(expected, result!.TestEnum);
    }

    [Theory]
    [InlineData("-1")]
    [InlineData("3")]
    public void Read_WhenJsonTokenTypeIsStringAndEnumValueNotDefined_ShouldThrowException(string input)
    {
        var options = GetJsonSerializerOptions(typeof(EnumJsonConverter<TestEnum>));
        var json = $"{{\"testEnum\": \"{input}\"}}";
        Assert.Throws<Exception>(() => JsonSerializer.Deserialize<TestModel>(json, options));
    }

    [Fact]
    public void Read_WhenJsonTokenTypeIsStringAndNotParsable_ShouldThrowException()
    {
        var options = GetJsonSerializerOptions(typeof(EnumJsonConverter<TestEnum>));
        var json = $"{{\"testEnum\": \"test\"}}";
        Assert.Throws<Exception>(() => JsonSerializer.Deserialize<TestModel>(json, options));
    }

    [Fact]
    public void Read_WhenJsonTokenTypeIsStringAndWhiteSpace_ShouldThrowException()
    {
        var options = GetJsonSerializerOptions(typeof(EnumJsonConverter<TestEnum>));
        var json = $"{{\"testEnum\": \"\"}}";
        Assert.Throws<Exception>(() => JsonSerializer.Deserialize<TestModel>(json, options));
    }

    [Theory]
    [InlineData(1, TestEnum.Value1)]
    [InlineData(2, TestEnum.Value2)]
    public void Read_WhenJsonTokenTypeIsNumberAndEnumValueDefined_ShouldReturnEnum(int input, TestEnum expected)
    {
        var options = GetJsonSerializerOptions(typeof(EnumJsonConverter<TestEnum>));
        var json = $"{{\"testEnum\": {input}}}";
        var result = JsonSerializer.Deserialize<TestModel>(json, options);
        Assert.Equal(expected, result!.TestEnum);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(3)]
    public void Read_WhenJsonTokenTypeIsNumberAndEnumValueNotDefined_ShouldThrowException(int input)
    {
        var options = GetJsonSerializerOptions(typeof(EnumJsonConverter<TestEnum>));
        var json = $"{{\"testEnum\": {input}}}";
        Assert.Throws<Exception>(() => JsonSerializer.Deserialize<TestModel>(json, options));
    }

    [Fact]
    public void Read_WhenJsonTokenTypeIsNull_ShouldThrowException()
    {
        var options = GetJsonSerializerOptions(typeof(EnumJsonConverter<TestEnum>));
        var json = $"{{\"testEnum\": null}}";
        Assert.Throws<Exception>(() => JsonSerializer.Deserialize<TestModel>(json, options));
    }

    [Fact]
    public void Read_WhenJsonTokenTypeIsNotSupported_ShouldThrowException()
    {
        var options = GetJsonSerializerOptions(typeof(EnumJsonConverter<TestEnum>));
        var json = $"{{\"testEnum\": true}}";
        Assert.Throws<Exception>(() => JsonSerializer.Deserialize<TestModel>(json, options));
    }

    [Fact]
    public void Write_ShouldReturnJson()
    {
        var value = new TestModel
        {
            TestEnum = TestEnum.Value1
        };
        var options = GetJsonSerializerOptions(typeof(EnumJsonConverter<TestEnum>));
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