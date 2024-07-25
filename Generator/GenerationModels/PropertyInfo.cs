namespace Generator.GenerationModels;

public sealed record PropertyInfo
{
    public string Name { get; set; } = string.Empty;

    public List<string> Associations { get; set; } = new();

    public string Type { get; set; } = string.Empty;

    public string TrimmedType => Type.TrimEnd('?');

    public string CollectionType { get; set; } = string.Empty;

    public string ItemType { get; set; } = string.Empty;

    public bool IsCollection { get; set; }

    public bool IsNullable { get; set; }

    public bool IsReferenceType { get; set; }

    public bool IsMapped { get; set; }
}