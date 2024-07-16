// See https://aka.ms/new-console-template for more information

using Generated;
using SourceGeneratorMapper;
using SourceGeneratorMapper.Test;


Console.WriteLine("Hello, World!1");

var x = new MapFrom { Id = 1, Name = "A" };
var y = x.Map();

[Map]
public class A
{
    public int Id { get; set; }

    public string Name { get; set; }
}

[Map]
public class B
{
    public int Id { get; set; }

    public string Name { get; set; }
}