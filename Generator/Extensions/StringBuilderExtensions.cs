using System.Text;

namespace Generator;

public static class StringBuilderExtensions
{
    public static void AppendUsings(this StringBuilder stringBuilder,
        MapperInfo mapperInfo)
    {
        var namespaces = mapperInfo.LookupClasses
            .Select(x => x.Namespace)
            .Distinct()
            .ToList();

        namespaces.Add(mapperInfo.MapToLookUpClass.Namespace);

        foreach (var @namespace in namespaces)
        {
            stringBuilder.Append("using ");
            stringBuilder.Append(@namespace);
            stringBuilder.Append(";\n");
        }
    }

    public static void AppendNamespace(this StringBuilder stringBuilder)
    {
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("namespace Generated;\n");
    }

    public static void AppendClassName(this StringBuilder stringBuilder,
        string className)
    {
        stringBuilder.Append("public static class ");
        stringBuilder.Append(className);
        stringBuilder.AppendLine();
        stringBuilder.Append('{');
        stringBuilder.AppendLine();
    }

    public static void AppendTab(this StringBuilder stringBuilder, int count = 1)
    {
        for (var i = 0; i < count; i++) stringBuilder.Append("\t");
    }

    public static void AppendMapperBody(this StringBuilder stringBuilder,
        MapperInfo mapperInfo)
    {
        var to = mapperInfo.MapToLookUpClass;
        var count = mapperInfo.LookupClasses.Count;

        for (var i = 0; i < count; i++)
        {
            var from = mapperInfo.LookupClasses[i];

            if (i != 0)
                stringBuilder.AppendLine();

            stringBuilder.AppendTab();
            stringBuilder.Append("public static ");
            stringBuilder.Append(to.Name);
            stringBuilder.Append(" Map(this ");
            stringBuilder.Append(from.Name);
            stringBuilder.Append(" from)");
            stringBuilder.AppendLine();
            stringBuilder.AppendTab();
            stringBuilder.Append('{');
            stringBuilder.AppendLine();
            stringBuilder.AppendTab(2);
            stringBuilder.Append("return new()");
            stringBuilder.AppendLine();
            stringBuilder.AppendTab(2);
            stringBuilder.Append('{');
            stringBuilder.AppendLine();

            foreach (var fromProperty in from.Properties)
            {
                var toProperty = to.Properties.FirstOrDefault(x => x == fromProperty);
                if (toProperty == null) continue;

                stringBuilder.AppendTab(3);
                stringBuilder.Append(toProperty);
                stringBuilder.Append(" = from.");
                stringBuilder.Append(fromProperty);
                stringBuilder.Append(',');
                stringBuilder.AppendLine();
            }

            stringBuilder.AppendTab(2);
            stringBuilder.Append("};");
            stringBuilder.AppendLine();
            stringBuilder.AppendTab();
            stringBuilder.Append('}');
            stringBuilder.AppendLine();
        }

        stringBuilder.Append('}');
    }
}