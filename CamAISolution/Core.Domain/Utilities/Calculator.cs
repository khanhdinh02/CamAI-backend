namespace Core.Domain.Utilities;

public static class Calculator
{
    public static T Divide<T>(T numerator, T denominator, T defaultValue = default)
        where T : struct
    {
        if (!typeof(T).IsNumericType())
        {
            throw new ArgumentException("Type T must be a numeric type.");
        }

        dynamic num = numerator;
        dynamic den = denominator;
        try
        {
            var result = num / den;
            return double.IsNaN((double)result) ? defaultValue : result;
        }
        catch (DivideByZeroException)
        {
            return defaultValue;
        }
    }
}
