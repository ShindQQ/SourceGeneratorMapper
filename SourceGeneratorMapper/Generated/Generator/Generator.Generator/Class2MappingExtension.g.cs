using SourceGeneratorMapper.Test;
using SourceGeneratorMapper;

namespace Generated;

public static class Class2MappingExtension
{
	public static Class2 MapToClass2(this Class1 from)
	{
		return new()
		{
			Id = from.Id,
			Name = from.Name,
			Description = from.Description,
			CreatedAt = from.CreatedAt,
			UpdatedAt = from.UpdatedAt,
			TestCollection1 = from.TestCollection1
				.MapToClass1()
				.ToList(),
			TestCollection2 = from.TestCollection2,
			RefType1 = from.RefType1,
			RefType2 = from.RefType2.MapToClass1(),
		};
	}

	public static IEnumerable<Class2> MapToClass2(this IEnumerable<Class1> from)
	{
		return from.Select(x => x.MapToClass2());
	}
}