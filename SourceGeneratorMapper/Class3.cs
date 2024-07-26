using SourceGeneratorMapper.Test;

namespace SourceGeneratorMapper;

[MapTo(typeof(Class1))]
[MapTo(typeof(Class2))]
public sealed class Class3
{
    public long Id { get; init; }

    public string Name { get; init; }

    public string? Description { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime? UpdatedAt { get; init; }
}