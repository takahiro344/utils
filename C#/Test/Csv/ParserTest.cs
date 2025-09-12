using System.Diagnostics.CodeAnalysis;

namespace Utils.test.Csv;

using Utils.Csv;
using Utils.Csv.ClassMaps;

[ExcludeFromCodeCoverage]
public class ParserTest
{
    [Fact]
    public void Test_Parse_WithoutClassMap()
    {
        var actual = Parser.Parse<Person>("Csv/data/persons-without-class-map.csv", true);

        var expected = new List<Person>()
        {
            new() { Name = "John", Age = 25, Height = 180m, Weight = 75m },
            new() { Name = "Tom", Age = 30, Height = 175m, Weight = 80m },
            new() { Name = "Alice", Age = 28, Height = 160m, Weight = 55m },
            new() { Name = "Bob", Age = 35, Height = 170m, Weight = 70m },
            new() { Name = "Emily", Age = 27, Height = 165m, Weight = 60m },
        };
        foreach (var (expectedPerson, actualPerson) in expected.Zip(actual))
        {
            Assert.Equal(expectedPerson.Name, actualPerson.Name);
            Assert.Equal(expectedPerson.Age, actualPerson.Age);
            Assert.Equal(expectedPerson.Height, actualPerson.Height);
            Assert.Equal(expectedPerson.Weight, actualPerson.Weight);
        }
    }

    [Fact]
    public void Test_Parse_WithClassMap()
    {
        var actual = Parser.Parse<Person>("Csv/data/persons-with-class-map.csv", true, classMap: new PersonMap());

        var expected = new List<Person>()
        {
            new() { Name = "John", Age = 25, Height = 180m, Weight = 75m },
            new() { Name = "Tom", Age = 30, Height = 175m, Weight = 80m },
            new() { Name = "Alice", Age = 28, Height = 160m, Weight = 55m },
            new() { Name = "Bob", Age = 35, Height = 170m, Weight = 70m },
            new() { Name = "Emily", Age = 27, Height = 165m, Weight = 60m },
        };
        foreach (var (expectedPerson, actualPerson) in expected.Zip(actual))
        {
            Assert.Equal(expectedPerson.Name, actualPerson.Name);
            Assert.Equal(expectedPerson.Age, actualPerson.Age);
            Assert.Equal(expectedPerson.Height, actualPerson.Height);
            Assert.Equal(expectedPerson.Weight, actualPerson.Weight);
        }
    }
}