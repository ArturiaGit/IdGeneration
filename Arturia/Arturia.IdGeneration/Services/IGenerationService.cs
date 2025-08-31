using System.Collections.Generic;
using Arturia.IdGeneration.Models;

namespace Arturia.IdGeneration.Services;

public interface IGenerationService
{
    public string Generate(GenerationOptions options);
}