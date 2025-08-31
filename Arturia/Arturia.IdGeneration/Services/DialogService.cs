using System;
using System.Threading.Tasks;
using Arturia.IdGeneration.ViewModels;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;

namespace Arturia.IdGeneration.Services;

public class DialogService(IServiceProvider provider) : IDialogService
{
    public void ShowWindow<TViewModel>(TViewModel viewModel,int seconds) where TViewModel : ViewModelBase
    {
        string? viewName = typeof(TViewModel).AssemblyQualifiedName?
            .Replace("ViewModel", "View",StringComparison.InvariantCulture)
            .Replace("ViewModels", "Views");
        if(viewName is null)
            throw new ArgumentNullException(nameof(viewName),"无法找到对应的View");

        Type? viewType = Type.GetType(viewName, throwOnError: false);
        if (viewType is null)
            throw new ArgumentNullException(nameof(viewType), "无法找到对应的View");

        Window? view = provider.GetRequiredService(viewType) as Window;
        if (view is null)
            throw new ArgumentNullException(nameof(view), "无法创建对应的View实例");
        
        view.DataContext = viewModel;
        view.ShowDialog(GetMainWindow());

        Task.Run(async () =>
        {
            await Task.Delay(seconds*1000);
            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                if (view.IsVisible)
                    view.Close();
            });
        });
    }
    
    private Window GetMainWindow()
        => App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow
            : throw new InvalidOperationException("无法获取主窗口");
}