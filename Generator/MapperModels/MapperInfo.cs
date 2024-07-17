using Generator.GenerationModels;

namespace Generator;

public sealed record MapperInfo
{
    public string MapTo { get; set; } = string.Empty;

    public LookupClass MapToLookUpClass { get; set; } = null!;

    public List<LookupClass> LookupClasses { get; set; } = [];
}