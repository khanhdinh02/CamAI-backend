using Microsoft.AspNetCore.Mvc;

namespace Host.CamAI.API.Attributes;

public class LookupHttpGetAttribute(string template, Type type) : HttpGetAttribute(template)
{
    public Type Type { get; } = type;
}
