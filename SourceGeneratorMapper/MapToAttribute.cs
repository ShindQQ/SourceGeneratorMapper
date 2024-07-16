namespace SourceGeneratorMapper;

public sealed class MapToAttribute : Attribute
{
    public MapToAttribute(params Type[] types)
    {
        Types = types;
    }

    public Type[] Types { get; init; }
}