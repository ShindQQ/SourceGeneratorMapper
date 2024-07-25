<div align="center">
  <h1>Simple Source Generator Mapper</h1>
</div>
<br/>
<p>This is a simple source generator to create mapping extension methods.</p>

## Overview

`SourceGeneratorMapper` is a source generator that automates the creation of mapping extension methods to map one type to another. This is particularly useful in scenarios where you need to convert between different data transfer objects (DTOs), view models, or any other types with similar properties.

## Features

- Automatically generates mapping extension methods to map one type to another.
- Allows for custom property mappings using attributes.

## Usage

### Mapping Types

To map one type to another, use the `MapTo` attribute and provide the target type you want to map to.

```csharp
[MapTo(typeof(TargetType))]
public class SourceType
{
    public string TargetPropertyName { get; set; }
    // Properties
}
```
### Mapping Properties

To map specific properties, use the MapToProperty attribute and provide the target property name you want to map to.

```csharp
public class SourceType
{
    [MapToProperty("TargetPropertyName")]
    public string SourcePropertyName { get; set; }
    // Properties
}
```

## Example
### Source Type

```csharp
[MapTo(typeof(TargetType))]
public class SourceType
{
    public int Id { get; set; }
    
    [MapToProperty("FullName")]
    public string Name { get; set; }
    
    public DateTime DateOfBirth { get; set; }
}
```

### Target Type

```csharp
public class TargetType
{
    public int Id { get; set; }

    public string FullName { get; set; }

    public DateTime DateOfBirth { get; set; }
}
```

### Generated Mapping Extension

```csharp
public static class SourceTypeMappingExtension
{
    public static TargetType MapToTargetType(this SourceType from)
    {
        return new TargetType
        {
            Id = from.Id,
            FullName = from.Name,
            DateOfBirth = from.DateOfBirth
        };
    }
}

```
