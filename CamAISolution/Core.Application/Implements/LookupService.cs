using System.Reflection;

namespace Core.Application.Implements;

public static class LookupService
{
    public static Dictionary<int, string> GetLookupValues(Type type)
    {
        if (!(type.IsSealed && type.Name.EndsWith("Enum")))
            throw new InvalidDataException("T must be static class that ends with 'Enum'");

        return type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => fi is { IsLiteral: true, IsInitOnly: false })
            .ToDictionary(x => (int)x.GetRawConstantValue()!, x => x.Name);
    }
}
