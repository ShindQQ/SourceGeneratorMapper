using Generator.Extensions;
using Generator.GenerationModels;
using Generator.StaticModels;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generator;

public class SyntaxReceiver : ISyntaxReceiver
{
    public List<LookupClass> ClassMappings { get; } = [];

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        switch (syntaxNode)
        {
            case RecordDeclarationSyntax recordDeclarationSyntax:
                AddMappings(recordDeclarationSyntax);
                break;
            case ClassDeclarationSyntax classDeclarationSyntax:
                AddMappings(classDeclarationSyntax);
                break;
        }
    }

    private void AddMappings(TypeDeclarationSyntax typeDeclarationSyntax)
    {
        var hasMapAttribute = typeDeclarationSyntax.AttributeLists
            .Any(x => x.Attributes.Any(y => y.Name.ToString().Equals(MappingAttribute.MapTo)));

        if (!hasMapAttribute) return;

        var mapTo = typeDeclarationSyntax.AttributeLists
            .SelectMany(x => x.Attributes)
            .Where(attributeSyntax => attributeSyntax.Name.ToString().Equals(MappingAttribute.MapTo))
            .Select(x => x.ArgumentList)
            .Where(x => x != null)
            .Select(x => x!.Arguments)
            .Select(arg => arg.ToString().ExtractTypeName())
            .ToList();

        var properties = typeDeclarationSyntax.Members
            .Select(x => x as PropertyDeclarationSyntax)
            .Where(x => x != null)
            .Select(x => new PropertyInfo
            {
                Name = x!.Identifier.Text,
                Associations = x.AttributeLists
                    .SelectMany(attributeListSyntax => attributeListSyntax.Attributes)
                    .Where(attributeSyntax => attributeSyntax.Name.ToString().Equals(MappingAttribute.MapToProperty))
                    .SelectMany(attributeSyntax =>
                        attributeSyntax.ArgumentList?.Arguments ?? Enumerable.Empty<AttributeArgumentSyntax>())
                    .Select(argument => argument.Expression)
                    .OfType<LiteralExpressionSyntax>()
                    .Select(literalExpression => literalExpression.Token.ValueText)
                    .ToList(),
                Type = x.Type.ToString(),
                CollectionType = x.Type.GetCollectionType(),
                CollectionItemType = x.Type.GetCollectionItemType(),
                IsCollection = x.Type.IsCollectionType(),
                IsNullable = x.Type.IsNullableType(),
                IsReferenceType = x.Type.IsReferenceType()
            })
            .ToList();

        ClassMappings.Add(new LookupClass
        {
            Name = typeDeclarationSyntax.Identifier.Text,
            OutputDirectory = typeDeclarationSyntax
                .AttributeLists
                .SelectMany(x => x.Attributes)
                .Where(y => y.Name.ToString().Equals(MappingAttribute.OutputDirectory))
                .Select(x => x.ArgumentList)
                .Where(x => x != null)
                .Select(x => x!.Arguments)
                .Select(arg => arg.ToString().ExtractDirectory())
                .FirstOrDefault() ?? string.Empty,
            Namespace = typeDeclarationSyntax.GetNamespace(),
            Properties = properties,
            MapTo = mapTo
        });
    }
}