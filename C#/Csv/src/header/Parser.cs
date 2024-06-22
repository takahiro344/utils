namespace Csv.src.header;

public static class Parser
{
    public static List<T> Parse<T>(string filePath) where T : new()
    {
        var lines = File.ReadAllLines(filePath);
        if (lines.Length == 0)
        {
            throw new Exception("Csv file is empty.");
        }

        var header = lines.First().Split(',');
        var data = lines.Skip(1).Select(l => l.Split(','));

        var properties = typeof(T).GetProperties();

        var result = new List<T>();
        foreach (var row in data)
        {
            var item = new T();

            for (int i = 0; i < header.Length; i++)
            {
                var property = properties.FirstOrDefault(p => p.Name == header[i]);
                property?.SetValue(item, Convert.ChangeType(row[i], property.PropertyType));
            }

            result.Add(item);
        }

        return result;
    }
}
