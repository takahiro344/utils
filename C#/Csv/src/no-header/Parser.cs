using System.Reflection;

namespace Csv;

using Csv.src.no_header;

public static class Parser
{
    private static readonly string[] _propertyNames =
    [
        "Name",
        "Age",
        "Height",
        "Weight"
    ];

    public static List<PersonEntity> Parse(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        if (lines.Length == 0)
        {
            throw new Exception("Csv file is empty.");
        }

        var lineStrings = lines.Select(l => l.Split(','));

        var properties = typeof(PersonEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var propertyInfos = new List<PropertyInfo>();
        for (int i = 0; i < _propertyNames.Length; ++i)
        {
            var property = properties.FirstOrDefault(p => p.Name == _propertyNames[i])
                ?? throw new Exception("Invalid property name.");
            propertyInfos.Add(property);
        }

        var personEntities = new List<PersonEntity>();
        foreach (var columns in lineStrings)
        {
            var personEntity = new PersonEntity();

            for (int columnIdx = 0; columnIdx < columns.Length; ++columnIdx)
            {
                var _ = TryParse(personEntity, columns[columnIdx], propertyInfos[columnIdx]);
            }

            personEntities.Add(personEntity);
        }

        return personEntities;
    }

    private static bool TryParse(PersonEntity personEntity, string column, PropertyInfo propertyInfo)
    {
        switch (propertyInfo.PropertyType.Name)
        {
            case "String":
                {
                    propertyInfo.SetValue(personEntity, column);
                    return true;
                }
            case "Int32":
                {
                    var parsed = int.TryParse(column, out var result);
                    propertyInfo.SetValue(personEntity, result);
                    return parsed;
                }
            case "Decimal":
                {
                    var parsed = decimal.TryParse(column, out var result);
                    propertyInfo.SetValue(personEntity, result);
                    return parsed;
                }
            default:
                throw new Exception("Invalid property type.");
        }

        throw new Exception("Invalid property name.");
    }
}