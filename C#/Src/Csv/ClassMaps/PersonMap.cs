using CsvHelper.Configuration;

namespace Utils.Csv.ClassMaps;

public class PersonMap : ClassMap<Person>
{
    public PersonMap()
    {
        Map(m => m.Name).Name("名前");
        Map(m => m.Age).Name("年齢");
        Map(m => m.Height).Name("身長(cm)");
        Map(m => m.Weight).Name("体重(kg)");
    }
}
