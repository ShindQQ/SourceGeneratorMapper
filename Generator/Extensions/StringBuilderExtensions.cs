using System.Text;
using Generator.GenerationModels;

namespace Generator.Extensions;

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

        foreach (var @namespace in namespaces) stringBuilder.AppendLine($"using {@namespace};");
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

            AppendMapOneToOne(stringBuilder, to, from);

            stringBuilder.AppendLine();

            AppendMapManyToMany(stringBuilder, to, from);
        }

        stringBuilder.Append('}');
    }

    private static void AppendMapOneToOne(StringBuilder stringBuilder, LookupClass to, LookupClass from)
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
            
            if (toPropertyInfo == null) continue;

            stringBuilder.AppendTab(3);
            stringBuilder.Append($"{toPropertyInfo.Name} = ");

            if (fromPropertyInfo.IsNullable)
                AppendNullablePropertyInfoMapping(stringBuilder, fromPropertyInfo, toPropertyInfo);
            else
                AppendNonNullablePropertyInfoMapping(stringBuilder, fromPropertyInfo, toPropertyInfo, 4);
        }

        stringBuilder.AppendTab(2);
        stringBuilder.AppendLine("};");
        stringBuilder.AppendTab();
        stringBuilder.AppendLine("}");
    }

    private static void AppendNullablePropertyInfoMapping(StringBuilder stringBuilder, PropertyInfo fromPropertyInfo,
        PropertyInfo toPropertyInfo)
    {
        if (toPropertyInfo.IsNullable)
        {
            stringBuilder.AppendLine($"from.{fromPropertyInfo.Name},");
            return;
        }

        stringBuilder.AppendLine($"from.{fromPropertyInfo.Name} != null ?");

        if (fromPropertyInfo.IsCollection)
            AppendCollectionMapping(stringBuilder, fromPropertyInfo, toPropertyInfo, 4);
        else if (fromPropertyInfo.TrimmedType.Equals(toPropertyInfo.TrimmedType))
            AppendSimpleMapping(stringBuilder, fromPropertyInfo, 4, true);
        else
            AppendComplexMapping(stringBuilder, fromPropertyInfo, toPropertyInfo, 4);
    }

    private static void AppendNonNullablePropertyInfoMapping(StringBuilder stringBuilder, PropertyInfo fromPropertyInfo,
        PropertyInfo toPropertyInfo, int tabLevel)
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
        else
        {
            stringBuilder.AppendLine($"from.{fromPropertyInfo.Name}.MapTo{toPropertyInfo.TrimmedType}(),");
        }
    }

    private static void AppendCollectionMapping(StringBuilder stringBuilder, PropertyInfo fromPropertyInfo,
        PropertyInfo toPropertyInfo, int tabLevel)
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

    private static void AppendSimpleMapping(StringBuilder stringBuilder, PropertyInfo fromPropertyInfo,
        int tabLevel, bool isNullable)
    {
        stringBuilder.AppendTab(tabLevel);
        stringBuilder.AppendLine($"from.{fromPropertyInfo.Name}{(isNullable ? ".Value" : "")} :");
        stringBuilder.AppendTab(tabLevel);
        stringBuilder.AppendLine("default,");
    }

    private static void AppendComplexMapping(StringBuilder stringBuilder, PropertyInfo fromPropertyInfo,
        PropertyInfo toPropertyInfo, int tabLevel)
    {
        stringBuilder.AppendTab(tabLevel);
        stringBuilder.AppendLine($"from.{fromPropertyInfo.Name}.MapTo{toPropertyInfo.ItemType}() :");
        stringBuilder.AppendTab(tabLevel);
        stringBuilder.AppendLine("null,");
    }

    private static void AppendMapManyToMany(StringBuilder stringBuilder, LookupClass to, LookupClass from)
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