using System.Collections.Generic;
using Arturia.IdGeneration.Models;
using Arturia.IdGeneration.ViewModels;
using Arturia.IdGeneration.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Arturia.IdGeneration.Services;

public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection services)
    {
        #region 注册视图
        services.AddSingleton<MainViewModel>();
        services.AddSingleton(r => new MainView
        {
            DataContext = r.GetRequiredService<MainViewModel>()
        });

        services.AddTransient<MessageBoxView>();
        services.AddTransient<DisclaimerView>();
        #endregion 注册视图

        #region 注册模型
        services.AddTransient<List<AreaModel>>();
        #endregion 注册模型
        
        #region 注册服务
        services.AddTransient<IAreaService, AreaService>();
        services.AddTransient<IDialogService, DialogService>();
        services.AddTransient<IGenerationService, GenerationService>();

        #endregion 注册服务
    }
}