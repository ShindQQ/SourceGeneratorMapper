namespace SourceGeneratorMapper.Test;

[MapTo(typeof(Class2))]
[MapTo(typeof(Class3))]
[OutputDirectory(@"Generated\test")]
public sealed record Class1
{
    public long Id { get; init; }

    public string Id1 { get; init; }

    public string? Id1Nullable { get; init; }

    public string Id1NonNullable { get; init; }

    public string Name { get; init; }

    public string? Description { get; init; }

    [MapToProperty("CreatedAt1")] public DateTime CreatedAt { get; init; }

    public DateTime? UpdatedAt { get; init; }

    public List<Class2> TestCollection1 { get; init; } = new();

    public List<Class2> TestCollection2 { get; init; } = new();

    public Class2[] TestCollection4 { get; init; } = null;

    public Class3? RefType1 { get; init; }

    public Class2 RefType2 { get; init; } = new();
}