using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generator.Extensions;

public static class TypeSyntaxExtensions
{
    public static bool IsCollectionType(this TypeSyntax type)
    {
        var typeName = type.ToString();
        return typeName.Contains("IEnumerable") || typeName.Contains("List") || typeName.Contains("Collection");
    }

    public static bool IsNullableType(this TypeSyntax type)
    {
        return type.ToString().EndsWith("?");
    }

    public static bool IsReferenceType(this TypeSyntax type)
    {
        var typeName = type.ToString();
        return char.IsUpper(typeName[0]);
    }
}