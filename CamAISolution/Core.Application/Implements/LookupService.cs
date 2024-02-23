using System.Reflection;
using Core.Domain.Models.Attributes;

namespace Core.Application.Implements;

public static class LookupService
{
    public static Dictionary<int, string> GetLookupValues(Type type)
    {
        if (!Attribute.IsDefined(type, typeof(LookupAttribute)))
            throw new InvalidDataException($"{type.Name} type doesn't have {nameof(LookupAttribute)} attribute");
        if (type.IsEnum)
            return Enum.GetNames(type).ToDictionary(s => (int)Enum.Parse(type, s), s => s);
        return type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => fi is { IsLiteral: true, IsInitOnly: false })
            .ToDictionary(x => (int)x.GetRawConstantValue()!, x => x.Name);
    }
}
