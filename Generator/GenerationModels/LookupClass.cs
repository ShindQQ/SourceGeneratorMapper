namespace Generator.GenerationModels;

public sealed record LookupClass
{
    public string Name { get; set; } = string.Empty;

    public string Namespace { get; set; } = string.Empty;

    public string OutputDirectory { get; set; } = string.Empty;

    public List<PropertyInfo> Properties { get; set; } = [];

    public List<string> MapTo { get; set; } = [];

    public string GetNamespaceFromDirectory()
    {
        var @namespace = string.Join(".", OutputDirectory.Split('\\').AsEnumerable());

        return @namespace;
    }
}