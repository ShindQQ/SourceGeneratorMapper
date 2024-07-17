using SourceGeneratorMapper.Test;

namespace SourceGeneratorMapper;

[MapTo(typeof(MapFrom))]
public sealed record MapTo
{
    public long Id { get; init; }
    
    public long? IdNullable { get; init; }
    
    public bool Bool { get; init; }

    public string Name { get; init; }

    public string? Description { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime? UpdatedAt { get; init; }
    
    public List<MapTo2>? TestCollection1 { get; init; }
    
    public List<MapTo2> TestCollection2 { get; init; } = new();
    
    public List<MapTo2> TestCollection3 { get; init; } = null;
    
    public MapTo2? RefType1 { get; init; }
    
    public MapTo2 RefType2 { get; init; } = new();
    
    public MapTo2 RefType3 { get; init; } = null;
}