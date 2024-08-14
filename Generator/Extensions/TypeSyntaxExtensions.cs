using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generator.Extensions;

public static class TypeSyntaxExtensions
{
    public static bool IsCollectionType(this TypeSyntax type)
    {
        var typeName = type.ToString();
        return typeName.Contains("IEnumerable") ||
               typeName.Contains("List") ||
               typeName.Contains("Collection") ||
               typeName.Contains("[]");
    }

    public static bool IsNullableType(this TypeSyntax type)
    {
        return type.ToString().EndsWith("?");
    }

    public static bool IsReferenceType(this TypeSyntax type)
    {
        return type switch
        {
            PredefinedTypeSyntax predefinedTypeSyntax => predefinedTypeSyntax.Keyword.Kind() switch
            {
                SyntaxKind.StringKeyword or SyntaxKind.ObjectKeyword => true,
                _ => false
            },
            ArrayTypeSyntax or NullableTypeSyntax or GenericNameSyntax => true,
            _ => false
        };
    }

    private static string GetBaseTypeName(this TypeSyntax type)
    {
        if (type is ArrayTypeSyntax)
            return "Array";

        var typeName = type.ToString().TrimEnd('?');
        if (!typeName.Contains('<'))
            return typeName;

        var genericPartIndex = typeName.IndexOf('<');
        return typeName.Substring(0, genericPartIndex);
    }

    public static string GetCollectionType(this TypeSyntax type)
    {
        var typeName = type.GetBaseTypeName();
        return type.IsCollectionType() ? typeName : string.Empty;
    }

    public static string GetCollectionItemType(this TypeSyntax type)
    {
        if (type is NullableTypeSyntax nullableType)
            type = nullableType.ElementType;

        return type switch
        {
            ArrayTypeSyntax arrayType => arrayType.ElementType.ToString(),
            GenericNameSyntax { TypeArgumentList.Arguments.Count: 1 } genericName => 
                genericName.TypeArgumentList.Arguments.First().ToString(),
            _ => string.Empty
        };
    }
}