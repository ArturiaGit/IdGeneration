using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace IdGeneration.Models;

public class AreaModel
{
    [JsonPropertyName("code")]
    public required string Code { get; init; } = string.Empty;
    
    [JsonPropertyName("name")]
    public required string Name { get; init; } = string.Empty;

    [JsonPropertyName("children")] 
    public IList<AreaModel>? Children { get; init; } = new List<AreaModel>();
}