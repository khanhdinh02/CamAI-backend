using System.Text.RegularExpressions;

namespace Core.Domain.Constants;

public static class RegexHelper
{
    /// <summary>
    /// <see cref="https://stackoverflow.com/a/3216204/19327184"/>
    /// </summary>
    public static readonly Regex PascalSplitting = new(@"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])");

    public static readonly Regex VietNamPhoneNumber = new(@"(03|05|07|08|09|01[2|6|8|9])+([0-9]{8})\b");

}