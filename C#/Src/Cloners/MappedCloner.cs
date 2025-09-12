using System.Reflection;

namespace Utils.Cloners;

/// <summary>
/// 同名プロパティ優先に加えてマップ補完による柔軟なコピーを提供するユーティリティ
/// </summary>
public static class MappedCloner
{
    public static TDest ClonePublicProperties<TDest, TSource>(
        this TDest destObject,
        TSource srcObject,
        Dictionary<string, string>? propertyMap = null,
        HashSet<string>? exclude = null)
    {
        return destObject.Clone(
            srcObject,
            BindingFlags.Public | BindingFlags.Instance,
            propertyMap,
            exclude);
    }

    public static TDest CloneAllProperties<TDest, TSource>(
        this TDest destObject,
        TSource srcObject,
        Dictionary<string, string>? propertyMap = null,
        HashSet<string>? exclude = null)
    {
        return destObject.Clone(
            srcObject,
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
            propertyMap,
            exclude);
    }

    private static TDest Clone<TDest, TSource>(
        this TDest destObject,
        TSource srcObject,
        BindingFlags bindingFlags,
        Dictionary<string, string>? propertyMap = null,
        HashSet<string>? exclude = null)
    {
        var srcType = typeof(TSource);
        var destType = typeof(TDest);

        foreach (var srcProperty in srcType.GetProperties(bindingFlags))
        {
            if (exclude != null && exclude.Contains(srcProperty.Name))
            {
                continue;
            }

            var destProperty = destType.GetProperty(srcProperty.Name, bindingFlags);
            if (destProperty == null &&
                propertyMap != null && propertyMap.TryGetValue(srcProperty.Name, out var mappedDestProperty))
            {
                destProperty = destType.GetProperty(mappedDestProperty, bindingFlags);
            }

            if (destProperty != null &&
                destProperty.CanWrite &&
                destProperty.PropertyType == srcProperty.PropertyType)
            {
                destProperty.SetValue(destObject, srcProperty.GetValue(srcObject));
            }
        }

        return destObject;
    }
}