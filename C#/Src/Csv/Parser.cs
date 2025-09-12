using CsvHelper.Configuration;
using System.Globalization;
using System.Text;

namespace Utils.Csv;

/// <summary>
/// ClassMap機能も使用可能ならCSVパーサー
/// </summary>
public static class Parser
{
    public static IEnumerable<T> Parse<T>(
        string filePath,
        bool hasHeaderRecord,
        CultureInfo? cultureInfo = null,
        ClassMap<T>? classMap = null) where T : class
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"{filePath} not found");
        }

        using var reader = new StreamReader(filePath, encoding: Encoding.UTF8);
        using var csv = new CsvHelper.CsvReader(reader, new CsvConfiguration(cultureInfo ?? CultureInfo.GetCultureInfo("ja-JP"))
        {
            HasHeaderRecord = hasHeaderRecord,
        });
        if (classMap != null)
        {
            csv.Context.RegisterClassMap(classMap);
        }

        foreach (var record in csv.GetRecords<T>())
        {
            yield return record;
        }
    }
}
