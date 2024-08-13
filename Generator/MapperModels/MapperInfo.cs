using Generator.GenerationModels;

namespace Generator.MapperModels;

public sealed record MapperInfo
{
    public string MapTo { get; set; } = string.Empty;

    public string GeneratedClassName => $"{MapTo}MappingExtension";

    public LookupClass MapToLookUpClass { get; set; } = null!;

    public List<LookupClass> LookupClasses { get; set; } = [];
}