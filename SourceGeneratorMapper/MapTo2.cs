using SourceGeneratorMapper.Test;

namespace SourceGeneratorMapper;

[MapTo(typeof(MapFrom))]
public sealed record MapTo2
{
    public long Id { get; init; }

    public string Name { get; init; }

    public string? Description { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime? UpdatedAt { get; init; }
}