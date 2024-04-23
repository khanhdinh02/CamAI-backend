namespace Core.Domain.Constants;

public static class StringExtensions
{
    public static string PascalCaseToSeparateWords(this string s) => string.Join(" ", RegexHelper.PascalSplitting.Split(s));

}