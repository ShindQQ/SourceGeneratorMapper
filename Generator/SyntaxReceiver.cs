using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generator;

public class SyntaxReceiver : ISyntaxReceiver
{
    public List<LookupClass> ClassMappings { get; } = [];

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not RecordDeclarationSyntax recordDeclarationSyntax) // ||
            //syntaxNode is not ClassDeclarationSyntax classDeclarationSyntax)
            return;

        AddMappings(recordDeclarationSyntax);
        //AddMappings(classDeclarationSyntax);
    }

    private void AddMappings(TypeDeclarationSyntax typeDeclarationSyntax)
    {
        var hasMapAttribute = typeDeclarationSyntax.AttributeLists
            .Any(x => x.Attributes.Any(y => y.Name.ToString() == "MapTo"));

        if (!hasMapAttribute) return;

        var mapTo = typeDeclarationSyntax.AttributeLists
            .SelectMany(x => x.Attributes)
            .Select(x => x.ArgumentList)
            .Where(x => x != null)
            .Select(x => x!.Arguments)
            .Select(arg => arg.ToString().ExtractTypeName())
            .ToList();

        var properties = typeDeclarationSyntax.Members
            .Select(x => x as PropertyDeclarationSyntax)
            .Where(x => x != null)
            .Select(x => x!.Identifier.Text)
            .ToList();

        ClassMappings.Add(new LookupClass
        {
            Name = typeDeclarationSyntax.Identifier.Text,
            Namespace = typeDeclarationSyntax.GetNamespace(),
            Properties = properties,
            MapTo = mapTo
        });
    }
}