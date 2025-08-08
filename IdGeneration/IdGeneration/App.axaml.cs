using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using IdGeneration.Models;
using IdGeneration.Services;
using IdGeneration.ViewModels;
using IdGeneration.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdGeneration;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    private static IConfigurationRoot Configuration { get; set; } = null!;

    public override void OnFrameworkInitializationCompleted()
    {
        BindingPlugins.DataValidators.RemoveAt(0);

        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        var collection = new ServiceCollection();
        collection.AddIdGenerationServices();

        collection.Configure<AreaModel>(Configuration.GetSection(nameof(AreaModel)));
        
        var serviceProvider = collection.BuildServiceProvider();
        
        MainWindowViewModel vm = serviceProvider.GetRequiredService<MainWindowViewModel>();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = vm
            };
        }

        try
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
        }
        catch (IOException)
        {
            System.Diagnostics.Debug.WriteLine("No console attached. Skipping Console.OutputEncoding configuration.");
        }
        base.OnFrameworkInitializationCompleted();
    }
}