namespace Generator.Extensions;

public static class StringExtensions
{
    public static string ExtractTypeName(this string input)
    {
        if (input.StartsWith("typeof(") && input.EndsWith(")"))
            return input.Substring(7, input.Length - 8);

        return string.Empty;
    }
}