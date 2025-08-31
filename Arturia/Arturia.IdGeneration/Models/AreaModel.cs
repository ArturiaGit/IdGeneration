using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Arturia.IdGeneration.Models;

public record AreaModel
{
    [JsonPropertyName("code")]
    public string Code { get; init; } = string.Empty;
    
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;
    
    [JsonPropertyName("children")]
    public ICollection<AreaModel>? Children { get; init; } = new List<AreaModel>();
}