using System.Text;
using Generator.Extensions;
using Microsoft.CodeAnalysis;

namespace Generator;

[Generator]
public class Generator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var syntaxReceiver = context.SyntaxReceiver as SyntaxReceiver;

        var setupMappers = SetupMappers(syntaxReceiver!);

        foreach (var setupMapper in setupMappers)
            context.AddSource($"{setupMapper.ClassName}.g.cs", setupMapper.MapperBody.ToString());
    }

    private static List<MapperCreationResult> SetupMappers(SyntaxReceiver syntaxReceiver)
    {
        var mapperInfos = syntaxReceiver.ClassMappings
            .SelectMany(lookupClass => lookupClass.MapTo,
                (lookupClass, mapTo) =>
                    new
                    {
                        LookupClass = lookupClass,
                        MapTo = mapTo
                    })
            .GroupBy(x => x.MapTo)
            .Select(grouping => new MapperInfo
            {
                MapTo = grouping.Key,
                MapToLookUpClass = syntaxReceiver.ClassMappings
                    .First(x => x.Name.Equals(grouping.Key)),
                LookupClasses = grouping
                    .Select(x => x.LookupClass)
                    .ToList()
            })
            .ToList();

        var result = new List<MapperCreationResult>();

        foreach (var mapperInfo in mapperInfos)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendUsings(mapperInfo);
            stringBuilder.AppendNamespace();

            var className = $"{mapperInfo.MapTo}MappingExtension";

            stringBuilder.AppendClassName(className);

            stringBuilder.AppendMapperBody(mapperInfo);

            result.Add(new MapperCreationResult
            {
                ClassName = className,
                MapperBody = stringBuilder
            });
        }

        return result;
    }
}