using System;
using System.Collections.Generic;
using Arturia.IdGeneration.Enums;

namespace Arturia.IdGeneration.Models;

public record GenerationOptions
{
    public string Location { get; init; } = string.Empty;
    public string LocationCode { get; init; } = string.Empty;
    public List<string> BirthDays { get; init; } = [];
    public int GenerationCount { get; init; } = 5;
    public GenderOptionEnum GenderOptionEnum { get; init; }
    public NameGenerationOptionEnum NameGenerationOptionEnum { get; init; }
}