using SourceGeneratorMapper.Test;

namespace SourceGeneratorMapper;

[MapTo(typeof(Class1))]
public sealed record Class2
{
    public long Id { get; init; }
    
    public long? IdNullable { get; init; }
    
    public bool Bool { get; init; }

    public string Name { get; init; }

    public string? Description { get; init; }

    public DateTime? CreatedAt { get; init; }

    public DateTime? UpdatedAt { get; init; }

    public List<Class1>? TestCollection1 { get; init; } 
    
    public List<Class3> TestCollection2 { get; init; } = new();
    
    public List<Class3> TestCollection3 { get; init; } = null;
    
    public Class3? RefType1 { get; init; }
    
    public Class1 RefType2 { get; init; } = new();
    
    public Class3 RefType3 { get; init; } = null;
}