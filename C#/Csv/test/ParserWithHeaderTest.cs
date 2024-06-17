using Xunit;

namespace Csv.Test;

using Csv;

public class ParserWithHeaderTest
{
    [Fact]
    public void Test_Parse()
    {
        var actual = ParserWithHeader.Parse<PersonEntity>("test/data/persons_with_header.csv");

        var expected = new List<PersonEntity>()
        {
            new() { Name = "John", Age = 25, Height = 180, Weight = 75 },
            new() { Name = "Tom", Age = 30, Height = 175, Weight = 80 },
            new() { Name = "Alice", Age = 28, Height = 160, Weight = 55 },
            new() { Name = "Bob", Age = 35, Height = 170, Weight = 70 },
            new() { Name = "Emily", Age = 27, Height = 165, Weight = 60 },
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