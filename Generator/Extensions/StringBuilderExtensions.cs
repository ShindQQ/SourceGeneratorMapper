using System.Text;
using Generator.GenerationModels;

namespace Generator.Extensions;

public static class StringBuilderExtensions
{
    private static readonly Dictionary<(string fromType, string toType), Func<string, string>> TypeConversions =
        new()
        {
            {
                ("string", "int"),
                from => $"int.TryParse(from.{from}, out var _{from}) ?\n\t\t\t\t _{from} :\n\t\t\t\t default"
            },
            { ("int", "string"), from => $"from.{from}.ToString()" },
            {
                ("string", "long"),
                from => $"long.TryParse(from.{from}, out var _{from}) ?\n\t\t\t\t _{from} :\n\t\t\t\t default"
            },
            { ("long", "string"), from => $"from.{from}.ToString()" },
            {
                ("string", "float"),
                from => $"float.TryParse(from.{from}, out var _{from}) ?\n\t\t\t\t _{from} :\n\t\t\t\t default"
            },
            { ("float", "string"), from => $"from.{from}.ToString()" },
            {
                ("string", "double"),
                from => $"double.TryParse(from.{from}, out var _{from}) ?\n\t\t\t\t _{from} :\n\t\t\t\t default"
            },
            { ("double", "string"), from => $"from.{from}.ToString()" },
            {
                ("string", "decimal"),
                from => $"decimal.TryParse(from.{from}, out var _{from}) ?\n\t\t\t\t _{from} :\n\t\t\t\t default"
            },
            { ("decimal", "string"), from => $"from.{from}.ToString()" },
            {
                ("string", "bool"),
                from => $"bool.TryParse(from.{from}, out var _{from}) ?\n\t\t\t\t _{from} :\n\t\t\t\t default"
            },
            { ("bool", "string"), from => $"from.{from}.ToString()" },
            {
                ("string", "DateTime"),
                from => $"DateTime.TryParse(from.{from}, out var _{from}) ?\n\t\t\t\t _{from} :\n\t\t\t\t default"
            },
            { ("DateTime", "string"), from => $"from.{from}.ToString(\"o\")" },
            { ("DateTime", "long"), from => $"from.{from}.Ticks" },
            { ("long", "DateTime"), from => $"new DateTime(from.{from})" }
        };

    public static void AppendUsings(this StringBuilder stringBuilder,
        MapperInfo mapperInfo)
    {
        var namespaces = mapperInfo.LookupClasses
            .Select(x => x.Namespace)
            .Distinct()
            .Concat(new[] { mapperInfo.MapToLookUpClass.Namespace })
            .ToList();

        foreach (var @namespace in namespaces)
            stringBuilder.AppendLine($"using {@namespace};");
    }

    public static void AppendNamespace(this StringBuilder stringBuilder)
    {
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("namespace Generated;");
        stringBuilder.AppendLine();
    }

    public static void AppendClassName(this StringBuilder stringBuilder,
        string className)
    {
        stringBuilder.AppendLine($"public static class {className}");
        stringBuilder.AppendLine("{");
    }

    public static void AppendTab(this StringBuilder stringBuilder, int count = 1)
    {
        stringBuilder.Append(new string('\t', count));
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

            stringBuilder.AppendMapOneToOne(to, from);

            stringBuilder.AppendLine();

            stringBuilder.AppendMapManyToMany(to, from);
        }

        stringBuilder.Append('}');
    }

    private static void AppendMapOneToOne(this StringBuilder stringBuilder, LookupClass to, LookupClass from)
    {
        stringBuilder.AppendTab();
        stringBuilder.AppendLine($"public static {to.Name} MapTo{to.Name}(this {from.Name} from)");
        stringBuilder.AppendTab();
        stringBuilder.AppendLine("{");
        stringBuilder.AppendTab(2);
        stringBuilder.AppendLine("return new()");
        stringBuilder.AppendTab(2);
        stringBuilder.AppendLine("{");

        foreach (var fromPropertyInfo in from.Properties)
        {
            var toPropertyInfo = to.Properties
                .FirstOrDefault(x => x.Name.Equals(fromPropertyInfo.Name) ||
                                     fromPropertyInfo.Associations.Contains(x.Name));

            if (toPropertyInfo == null || toPropertyInfo.IsMapped) continue;

            stringBuilder.AppendTab(3);
            stringBuilder.Append($"{toPropertyInfo.Name} = ");

            if (fromPropertyInfo.IsNullable)
                stringBuilder.AppendNullablePropertyInfoMapping(fromPropertyInfo, toPropertyInfo);
            else
                stringBuilder.AppendNonNullablePropertyInfoMapping(fromPropertyInfo, toPropertyInfo);

            toPropertyInfo.IsMapped = true;
        }

        stringBuilder.AppendTab(2);
        stringBuilder.AppendLine("};");
        stringBuilder.AppendTab();
        stringBuilder.AppendLine("}");
    }

    private static void AppendNullablePropertyInfoMapping(this StringBuilder stringBuilder,
        PropertyInfo fromPropertyInfo,
        PropertyInfo toPropertyInfo)
    {
        if (toPropertyInfo.IsNullable)
        {
            if (TypeConversions.TryGetValue((fromPropertyInfo.TrimmedType, toPropertyInfo.TrimmedType),
                    out var conversion))
                stringBuilder.AppendLine($"{conversion($"{fromPropertyInfo.Name}")},");
            else
                stringBuilder.AppendLine($"from.{fromPropertyInfo.Name},");

            return;
        }

        stringBuilder.AppendLine($"from.{fromPropertyInfo.Name} != null ?");

        if (fromPropertyInfo.IsCollection)
        {
            stringBuilder.AppendCollectionMapping(fromPropertyInfo, toPropertyInfo);
        }
        else if (fromPropertyInfo.TrimmedType.Equals(toPropertyInfo.TrimmedType))
        {
            stringBuilder.AppendSimpleMapping(fromPropertyInfo, true);
        }
        else if (TypeConversions.TryGetValue((fromPropertyInfo.TrimmedType, toPropertyInfo.TrimmedType),
                     out var conversion))
        {
            stringBuilder.AppendTab(4);
            stringBuilder.AppendLine($"{conversion($"{fromPropertyInfo.Name}")} :");
            stringBuilder.AppendTab(4);
            stringBuilder.AppendLine("default,");
        }
        else
        {
            stringBuilder.AppendComplexMapping(fromPropertyInfo, toPropertyInfo);
        }
    }

    private static void AppendNonNullablePropertyInfoMapping(this StringBuilder stringBuilder,
        PropertyInfo fromPropertyInfo,
        PropertyInfo toPropertyInfo, int tabLevel = 4)
    {
        if (fromPropertyInfo.IsCollection)
        {
            if (fromPropertyInfo.CollectionType.Equals(toPropertyInfo.CollectionType) &&
                fromPropertyInfo.ItemType.Equals(toPropertyInfo.ItemType))
            {
                stringBuilder.AppendLine($"from.{fromPropertyInfo.Name},");
            }
            else
            {
                stringBuilder.AppendLine($"from.{fromPropertyInfo.Name}");
                stringBuilder.AppendTab(tabLevel);
                stringBuilder.AppendLine($".MapTo{toPropertyInfo.ItemType}()");
                stringBuilder.AppendTab(tabLevel);
                stringBuilder.AppendLine($".To{toPropertyInfo.CollectionType}(),");
            }
        }
        else if (fromPropertyInfo.TrimmedType.Equals(toPropertyInfo.TrimmedType))
        {
            stringBuilder.AppendLine($"from.{fromPropertyInfo.Name},");
        }
        else if (TypeConversions.TryGetValue((fromPropertyInfo.TrimmedType, toPropertyInfo.TrimmedType),
                     out var conversion))
        {
            stringBuilder.AppendLine($"{conversion($"{fromPropertyInfo.Name}")},");
        }
        else
        {
            stringBuilder.AppendLine($"from.{fromPropertyInfo.Name}.MapTo{toPropertyInfo.TrimmedType}(),");
        }
    }

    private static void AppendCollectionMapping(this StringBuilder stringBuilder, PropertyInfo fromPropertyInfo,
        PropertyInfo toPropertyInfo, int tabLevel = 4)
    {
        stringBuilder.AppendTab(tabLevel);

        if (fromPropertyInfo.CollectionType.Equals(toPropertyInfo.CollectionType) &&
            fromPropertyInfo.ItemType.Equals(toPropertyInfo.ItemType))
        {
            stringBuilder.AppendLine($"from.{fromPropertyInfo.Name} :");
        }
        else
        {
            stringBuilder.AppendLine($"from.{fromPropertyInfo.Name}");
            stringBuilder.AppendTab(tabLevel);
            stringBuilder.AppendLine($".MapTo{toPropertyInfo.ItemType}()");
            stringBuilder.AppendTab(tabLevel);
            stringBuilder.AppendLine($".To{toPropertyInfo.CollectionType}() :");
        }

        stringBuilder.AppendTab(tabLevel);
        stringBuilder.AppendLine("new(),");
    }

    private static void AppendSimpleMapping(this StringBuilder stringBuilder, PropertyInfo fromPropertyInfo,
        bool isNullable, int tabLevel = 4)
    {
        stringBuilder.AppendTab(tabLevel);
        stringBuilder.AppendLine($"from.{fromPropertyInfo.Name}{(isNullable ? ".Value" : "")} :");
        stringBuilder.AppendTab(tabLevel);
        stringBuilder.AppendLine("default,");
    }

    private static void AppendComplexMapping(this StringBuilder stringBuilder, PropertyInfo fromPropertyInfo,
        PropertyInfo toPropertyInfo, int tabLevel = 4)
    {
        stringBuilder.AppendTab(tabLevel);
        stringBuilder.AppendLine($"from.{fromPropertyInfo.Name}.MapTo{toPropertyInfo.ItemType}() :");
        stringBuilder.AppendTab(tabLevel);
        stringBuilder.AppendLine("null,");
    }

    private static void AppendMapManyToMany(this StringBuilder stringBuilder, LookupClass to, LookupClass from)
    {
        stringBuilder.AppendTab();
        stringBuilder.AppendLine(
            $"public static IEnumerable<{to.Name}> MapTo{to.Name}(this IEnumerable<{from.Name}> from)");
        stringBuilder.AppendTab();
        stringBuilder.AppendLine("{");
        stringBuilder.AppendTab(2);
        stringBuilder.AppendLine($"return from.Select(x => x.MapTo{to.Name}());");
        stringBuilder.AppendTab();
        stringBuilder.AppendLine("}");
    }
}