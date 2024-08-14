using SourceGeneratorMapper.Test;
using SourceGeneratorMapper;
using Generated.test;
using Generated;

namespace Generated;

public static class Class3MappingExtension
{
	public static Class3 MapToClass3(this Class1 from)
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

	public static IEnumerable<Class3> MapToClass3(this IEnumerable<Class1> from)
	{
		return from.Select(x => x.MapToClass3());
	}

	public static Class3 MapToClass3(this Class2 from)
	{
		return new()
		{
		};
	}

	public static IEnumerable<Class3> MapToClass3(this IEnumerable<Class2> from)
	{
		return from.Select(x => x.MapToClass3());
	}
}