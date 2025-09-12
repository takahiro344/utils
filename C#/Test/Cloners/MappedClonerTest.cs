using System.Diagnostics.CodeAnalysis;

namespace Utils.Tests.Converters;

using Utils.Cloners;

[ExcludeFromCodeCoverage]
public class MappedClonerTest
{
    public class TestClass1
    {
        public int Property1 { get; set; }
        public int Property2 { get; set; }
        public int Property3 { get; set; }
    }

    public class TestClass2
    {
        public int Property1 { get; set; }
        public decimal Property2 { get; set; }
        public int Property4 { get; set; }
    }

    public class TestClass3
    {
        public int PropertyA { get; set; }
        public int PropertyB { get; set; }
        public int PropertyC { get; set; }
    }


    [Fact]
    public void Clone_WhenPropertiesCloneInhibitedIsNotIncluded_ShouldSuccess()
    {
        var t2 = new TestClass2()
        {
            Property1 = 1,
            Property2 = 2.2m,
            Property4 = 6,
        };
        var t1 = new TestClass1()
        {
            Property1 = 3,
            Property2 = 4,
            Property3 = 5,
        }.ClonePublicProperties(t2);

        Assert.Equal(1, t1.Property1);
        Assert.Equal(4, t1.Property2);
        Assert.Equal(5, t1.Property3);
    }

    [Fact]
    public void Clone_WhenPropertiesCloneInhibitedIsIncluded_ShouldSuccess()
    {
        var t2 = new TestClass2()
        {
            Property1 = 1,
            Property2 = 2,
        };
        var t1 = new TestClass1()
        {
            Property1 = 3,
            Property2 = 4,
        }.ClonePublicProperties(t2, exclude: [
            "Property2"
        ]);

        Assert.Equal(1, t1.Property1);
        Assert.Equal(4, t1.Property2);
    }

    [Fact]
    public void Clone_WhenpropertyMapIsIncluded_ShouldSuccess()
    {
        var propertyMap = new Dictionary<string, string>()
        {
            { "PropertyA", "Property1" },
            { "PropertyB", "Property2" },
            { "PropertyC", "Property3" },
        };
        var t3 = new TestClass3()
        {
            PropertyA = 1,
            PropertyB = 2,
            PropertyC = 3,
        };
        var t1 = new TestClass1().ClonePublicProperties(t3, propertyMap: propertyMap);

        Assert.Equal(1, t1.Property1);
        Assert.Equal(2, t1.Property2);
        Assert.Equal(3, t1.Property3);
    }
}
