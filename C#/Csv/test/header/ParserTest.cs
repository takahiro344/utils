using Xunit;

namespace Csv.test.header;

using Csv.src.header;

public class ParserTest
{
    [Fact]
    public void Test_Parse()
    {
        var actual = Parser.Parse<PersonEntity>("test/header/data/persons_with_header.csv");

        var expected = new List<PersonEntity>()
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