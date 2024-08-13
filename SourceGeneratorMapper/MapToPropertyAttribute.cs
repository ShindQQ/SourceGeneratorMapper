namespace SourceGeneratorMapper;

[AttributeUsage(AttributeTargets.Property)]
public sealed class MapToPropertyAttribute : Attribute
{
    public MapToPropertyAttribute(params string[] sourcePropertyNames)
    {
        SourcePropertyNames = sourcePropertyNames;
    }

    public string[] SourcePropertyNames { get; init; }
}