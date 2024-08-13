using System.Text;

namespace Generator.MapperModels;

public sealed record MapperCreationResult
{
    public string ClassName { get; set; } = string.Empty;

    public string OutputDirectory { get; set; } = string.Empty;

    public StringBuilder MapperBody { get; set; } = null!;
}