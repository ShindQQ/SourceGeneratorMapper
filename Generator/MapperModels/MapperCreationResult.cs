using System.Text;

namespace Generator;

public sealed record MapperCreationResult
{
    public string ClassName { get; set; } = string.Empty;

    public StringBuilder MapperBody { get; set; } = null!;
}