namespace SourceGeneratorMapper;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class MapToAttribute : Attribute
{
    public MapToAttribute(params Type[] types)
    {
        Types = types;
    }

    public Type[] Types { get; init; }
}