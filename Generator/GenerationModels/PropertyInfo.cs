﻿namespace Generator.GenerationModels;

public sealed record PropertyInfo
{
    public string Name { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;
    
    public bool IsCollection { get; set; }
    
    public bool IsNullable { get; set; }
    
    public bool IsReferenceType { get; set; }
}