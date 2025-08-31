using System.Collections;
using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Arturia.IdGeneration.Models;
using Arturia.IdGeneration.Services;
using Avalonia.Markup.Xaml;
using Arturia.IdGeneration.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Arturia.IdGeneration;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private static IConfigurationRoot Configuration { get; set; } = null!;

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();

            string jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            var builder = new ConfigurationBuilder()
                .SetBasePath(jsonPath)
                .AddJsonFile("areas.json", optional: true, reloadOnChange: true);

            var collection = new ServiceCollection();
            collection.AddCommonServices();

            Configuration = builder.Build();
            collection.Configure<List<AreaModel>>(Configuration.GetSection("Areas"));
            
            var services = collection.BuildServiceProvider();
            desktop.MainWindow = services.GetRequiredService<MainView>();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}