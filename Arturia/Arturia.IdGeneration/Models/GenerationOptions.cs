using System;
using System.Collections.Generic;
using Arturia.IdGeneration.Enums;

namespace Arturia.IdGeneration.Models;

public record GenerationOptions
{
    public List<string> Locations { get; init; } = [];
    public List<string> LocationCodes { get; init; } = [];
    public List<string> BirthDays { get; init; } = [];
    public int GenerationCount { get; init; } = 5;
    public GenderOptionEnum GenderOptionEnum { get; init; }
    public NameGenerationOptionEnum NameGenerationOptionEnum { get; init; }
}