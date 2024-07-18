using SourceGeneratorMapper;
using SourceGeneratorMapper.Test;

namespace Generated;

public static class Class1MappingExtension
{
	public static Class1 MapToClass1(this Class2 from)
	{
		return new()
		{
			Id = from.Id,
			Name = from.Name,
			Description = from.Description,
			CreatedAt = from.CreatedAt != null ?
				from.CreatedAt.Value :
				default,
			UpdatedAt = from.UpdatedAt,
			TestCollection1 = from.TestCollection1 != null ?
				from.TestCollection1
				.MapToClass2()
				.ToList() :
				new(),
			TestCollection2 = from.TestCollection2,
			RefType1 = from.RefType1,
			RefType2 = from.RefType2.MapToClass2(),
		};
	}

	public static IEnumerable<Class1> MapToClass1(this IEnumerable<Class2> from)
	{
		return from.Select(x => x.MapToClass1());
	}

	public static Class1 MapToClass1(this Class3 from)
	{
		return new()
		{
			Id = from.Id,
			Name = from.Name,
			Description = from.Description,
			CreatedAt = from.CreatedAt,
			UpdatedAt = from.UpdatedAt,
		};
	}

	public static IEnumerable<Class1> MapToClass1(this IEnumerable<Class3> from)
	{
		return from.Select(x => x.MapToClass1());
	}
}