using IdGeneration.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace IdGeneration.Services;

public static class ServiceCollectionExtensions
{
    public static void AddIdGenerationServices(this IServiceCollection services)
    {
        services.AddSingleton<MainWindowViewModel>();
    }
}