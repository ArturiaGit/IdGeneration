using System;
using System.Threading;
using System.Threading.Tasks;
using Arturia.IdGeneration.ViewModels;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;

namespace Arturia.IdGeneration.Services;

public class DialogService(IServiceProvider provider) : IDialogService
{
    public async Task ShowWindowAsync<TViewModel>(TViewModel viewModel,int seconds=0) where TViewModel : ViewModelBase
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

        var cts = seconds > 0 ? new CancellationTokenSource(TimeSpan.FromSeconds(seconds)) : null;
        cts?.Token.Register(() =>
        {
            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                if (view.IsVisible)
                    view.Close();
            });
        },useSynchronizationContext:false);
        try
        {
            await view.ShowDialog(GetMainWindow());
        }
        finally
        {
            cts?.Dispose();
        }
    }
    
    private static Window GetMainWindow()
        => App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow
            : throw new InvalidOperationException("无法获取主窗口");
}