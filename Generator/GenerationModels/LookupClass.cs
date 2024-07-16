namespace Generator;

public sealed record LookupClass
{
    public string Name { get; set; } = string.Empty;

    public string Namespace { get; set; } = string.Empty;

    public List<string> Properties { get; set; } = [];

    public List<string> MapTo { get; set; } = [];
}