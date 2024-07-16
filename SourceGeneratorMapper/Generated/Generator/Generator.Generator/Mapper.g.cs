using SourceGeneratorMapper;
using SourceGeneratorMapper.Test;

namespace Generated;
public static class Mapper 
{

public static MapFrom Map(this MapTo from)
{
return new()
{
Id=from.Id,
Name=from.Name,
Description=from.Description,
CreatedAt=from.CreatedAt,
UpdatedAt=from.UpdatedAt,
};
}

public static MapTo Map(this MapFrom from)
{
return new()
{
Id=from.Id,
Name=from.Name,
Description=from.Description,
CreatedAt=from.CreatedAt,
UpdatedAt=from.UpdatedAt,
};
}
}