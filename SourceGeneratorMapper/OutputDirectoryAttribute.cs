namespace SourceGeneratorMapper;

[AttributeUsage(AttributeTargets.Class)]
public sealed class OutputDirectoryAttribute : Attribute
{
    public OutputDirectoryAttribute(string outputDirectory)
    {
        OutputDirectory = outputDirectory;
    }

    public string OutputDirectory { get; init; }
}