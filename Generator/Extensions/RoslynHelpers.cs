using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generator;

public static class RoslynHelpers
{
    public static string GetNamespace(this TypeDeclarationSyntax typeDeclaration)
    {
        var namespaces = new List<string>();

        var parent = typeDeclaration.Parent;

        while (parent != null)
        {
            if (parent is NamespaceDeclarationSyntax namespaceDeclaration)
                namespaces.Add(namespaceDeclaration.Name.ToString());
            else if (parent is FileScopedNamespaceDeclarationSyntax fileScopedNamespaceDeclaration)
                namespaces.Add(fileScopedNamespaceDeclaration.Name.ToString());
            else if (parent is CompilationUnitSyntax) break;

            parent = parent.Parent;
        }

        namespaces.Reverse();
        return string.Join(".", namespaces);
    }
}