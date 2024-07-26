using SourceGeneratorMapper;
using SourceGeneratorMapper.Test;
using SourceGeneratorMapper;

namespace Generated;

public static class Class2MappingExtension
{
	public static Class2 MapToClass2(this Class3 from)
	{
		return new()
		{
			Id = from.Id,
			Name = from.Name,
			Description = from.Description,
			UpdatedAt = from.UpdatedAt,
		};
	}

	public static IEnumerable<Class2> MapToClass2(this IEnumerable<Class3> from)
	{
		return from.Select(x => x.MapToClass2());
	}

	public static Class2 MapToClass2(this Class1 from)
	{
		return new()
		{
			Id1 = long.TryParse(from.Id1, out var _Id1) ?
				 _Id1 :
				 default,
			Id1Nullable = long.TryParse(from.Id1Nullable, out var _Id1Nullable) ?
				 _Id1Nullable :
				 default,
			CreatedAt1 = from.CreatedAt,
			TestCollection1 = from.TestCollection1
				.MapToClass1()
				.ToList(),
			TestCollection2 = from.TestCollection2
				.MapToClass3()
				.ToList(),
			TestCollection4 = from.TestCollection4
				.MapToClass3()
				.ToArray(),
			RefType1 = from.RefType1,
			RefType2 = from.RefType2.MapToClass1(),
		};
	}

	public static IEnumerable<Class2> MapToClass2(this IEnumerable<Class1> from)
	{
		return from.Select(x => x.MapToClass2());
	}
}