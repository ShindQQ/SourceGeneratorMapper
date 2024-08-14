using System.Text;
using Generator.Extensions;
using Generator.MapperModels;
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
        {
#pragma warning disable RS1035
            Directory.CreateDirectory(setupMapper.OutputDirectory);
            var file = new FileInfo(setupMapper.OutputDirectory);
            file.Directory?.Create();
            File.WriteAllText($@"{setupMapper.OutputDirectory}\{setupMapper.ClassName}.cs",
                setupMapper.MapperBody.ToString(),
                Encoding.UTF8);
#pragma warning restore RS1035
        }
    }

    private static List<MapperCreationResult> SetupMappers(SyntaxReceiver syntaxReceiver)
    {
        var mapperInfos = GetMapperInfos(syntaxReceiver);

        return mapperInfos.Select(SetupCreationResult).ToList();
    }

    private static List<MapperInfo> GetMapperInfos(SyntaxReceiver syntaxReceiver)
    {
        return syntaxReceiver.ClassMappings
            .SelectMany(lookupClass => lookupClass.MapTo,
                (lookupClass, mapTo) => new { LookupClass = lookupClass, MapTo = mapTo })
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
    }

    private static MapperCreationResult SetupCreationResult(MapperInfo mapperInfo)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.AppendUsings(mapperInfo);
        stringBuilder.AppendNamespace(mapperInfo.MapToLookUpClass.OutputDirectory);
        stringBuilder.AppendClassName(mapperInfo.GeneratedClassName);
        stringBuilder.AppendMapperBody(mapperInfo);

        return new MapperCreationResult
        {
            ClassName = mapperInfo.GeneratedClassName,
            OutputDirectory = mapperInfo.MapToLookUpClass.OutputDirectory,
            MapperBody = stringBuilder
        };
    }
}