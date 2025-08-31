using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;

namespace Arturia.IdGeneration.Models;

public class GenerationResult
{
    public string IdCard { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Gender { get; init; } = string.Empty;
    public int Age { get; init; }
    public string BirthDate { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty;
}

[JsonSerializable(typeof(List<GenerationResult>))]
public partial class SourceGenerationContext : JsonSerializerContext
{
    
}