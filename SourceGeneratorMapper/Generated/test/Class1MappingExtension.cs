using SourceGeneratorMapper;
using SourceGeneratorMapper.Test;

namespace Generated.test;

public static class Class1MappingExtension
{
    public static Class1 MapToClass1(this Class2 from)
    {
        return new Class1
        {
            Id = from.Id,
            Id1 = from.Id1.ToString(),
            Id1Nullable = from.Id1Nullable.ToString(),
            Id1NonNullable = from.IdNullable != null ? from.IdNullable.ToString() : default,
            Name = from.Name,
            Description = from.Description,
            CreatedAt = from.CreatedAt1 != null ? from.CreatedAt1.Value : default,
            UpdatedAt = from.UpdatedAt,
            TestCollection1 = from.TestCollection1 != null
                ? from.TestCollection1
                    .MapToClass2()
                    .ToList()
                : new List<Class2>(),
            TestCollection2 = from.TestCollection2
                .MapToClass2()
                .ToList(),
            TestCollection4 = from.TestCollection4
                .MapToClass2()
                .ToArray(),
            RefType1 = from.RefType1,
            RefType2 = from.RefType2.MapToClass2()
        };
    }

    public static IEnumerable<Class1> MapToClass1(this IEnumerable<Class2> from)
    {
        return from.Select(x => x.MapToClass1());
    }

    public static Class1 MapToClass1(this Class3 from)
    {
        return new Class1();
    }

    public static IEnumerable<Class1> MapToClass1(this IEnumerable<Class3> from)
    {
        return from.Select(x => x.MapToClass1());
    }
}