using System.Text;
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
        
        var namespaces = syntaxReceiver!
            .ClassMappings.Select(x => x.Namespace)
            .Distinct()
            .ToList();

        var stringBuilder = new StringBuilder();

        foreach (var @namespace in namespaces)
        {
            stringBuilder.Append("using ");
            stringBuilder.Append(@namespace);
            stringBuilder.Append(";\n");
        }

        stringBuilder.AppendLine();
        stringBuilder.Append("namespace Generated;");
        stringBuilder.AppendLine();

        stringBuilder.Append("public static class Mapper \n{\n");
        for (var i = 0; i < syntaxReceiver!.ClassMappings.Count; i++)
        {
            var from = syntaxReceiver.ClassMappings[i];

            for (var j = 0; j < syntaxReceiver.ClassMappings.Count; j++)
            {
                if (i == j) continue;

                var to = syntaxReceiver.ClassMappings[j];
                stringBuilder.AppendLine();
                stringBuilder.Append("public static ");
                stringBuilder.Append(to.Name);
                stringBuilder.Append(" Map(this ");
                stringBuilder.Append(from.Name);
                stringBuilder.Append(" from)\n{\n");
                stringBuilder.Append("return new()\n{\n");

                foreach (var fromProperty in from.Properties)
                {
                    var toProperty = to.Properties.FirstOrDefault(x => x == fromProperty);
                    if (toProperty == null) continue;

                    stringBuilder.Append(toProperty);
                    stringBuilder.Append("=from.");
                    stringBuilder.Append(fromProperty);
                    stringBuilder.Append(",\n");
                }

                stringBuilder.Append("};\n");
                stringBuilder.Append("}\n");
            }
        }

        stringBuilder.Append('}');

        context.AddSource("Mapper.g.cs", stringBuilder.ToString());
    }
}