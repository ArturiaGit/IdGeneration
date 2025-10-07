using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Arturia.IdGeneration.Enums;
using Arturia.IdGeneration.Models;
using Arturia.IdGeneration.Services;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Arturia.IdGeneration.ViewModels;

public partial class MainViewModel(IAreaService areaService, IDialogService dialogService,IGenerationService generationService) : ViewModelBase
{
    #region 私有字段
    private static readonly Random Random = new(Guid.NewGuid().GetHashCode());
    
    private readonly int[] _maleDigits = [1, 3, 5, 7, 9];
    private readonly int[] _femaleDigits = [0, 2, 4, 6, 8];

    private string _code = string.Empty;
    #endregion
    
    #region 通知属性
    [ObservableProperty]
    private ObservableCollection<AreaModel> _provinces =  new();
    
    [ObservableProperty]
    private ObservableCollection<AreaModel> _cities = new();
    
    [ObservableProperty]
    private ObservableCollection<AreaModel> _counties = new();
    
    [ObservableProperty] 
    private int _selectedProvinceIndex = -1;
    
    [ObservableProperty]
    private int _selectedCityIndex = -1;
    
    [ObservableProperty]
    private int _selectedDistrictIndex = -1;
    
    [ObservableProperty]
    private string _birthDate = string.Empty;

    [ObservableProperty]
    private int _startAge = 18;

    [ObservableProperty]
    private int _endAge = 25;

    [ObservableProperty] private int _generationCount = 5;

    [ObservableProperty] 
    private string _resultJson = string.Empty;
    
    [ObservableProperty]
    private bool _isVisible = true;
    
    [ObservableProperty]
    private GenderOptionEnum _currentGenderOption = GenderOptionEnum.Male;
    
    [ObservableProperty]
    private BirthDateOptionEnum _currentBirthDateOptionType = BirthDateOptionEnum.BySpecificDate;
    
    [ObservableProperty]
    private NameGenerationOptionEnum _currentNameType = NameGenerationOptionEnum.None;
    #endregion 通知属性
    
    #region 命令
    [RelayCommand]
    private async Task LoadProvinces()
    {
        bool? result = await dialogService.ShowWindowAsync(new DisclaimerViewModel()) as bool?;
        if (result is not true)
        {
            Environment.Exit(0);
            return;
        }

        IsVisible = result is false;
        Provinces.Clear();
        Provinces.Insert(0,new AreaModel
        {
            Name = "请选择省",
            Code = string.Empty,
            Children = new List<AreaModel>()
        });
        
        SelectedProvinceIndex = 0;
        
        ICollection<AreaModel> provinces = areaService.GetProvinces();
        foreach (AreaModel province in provinces)
            Provinces.Add(province);
    }

    [RelayCommand]
    private void LoadCities(AreaModel? province)
    {
        Cities.Clear();
        Cities.Insert(0,new AreaModel
        {
            Name = "请选择市",
            Code = string.Empty,
            Children = new List<AreaModel>()
        });
        SelectedCityIndex = 0;

        if (province is null || string.IsNullOrEmpty(province.Code))
            return;

        ICollection<AreaModel> cities = areaService.GetCities(province.Name);
        foreach (AreaModel city in cities)
            Cities.Add(city);
    }

    [RelayCommand]
    private void LoadDistricts(List<string> parameter)
    {
        Counties.Clear();
        Counties.Insert(0,new  AreaModel
        {
            Name = "请选择县/区",
            Code = string.Empty,
            Children = new List<AreaModel>()
        });
        SelectedDistrictIndex = 0;
        
        if(parameter.Count == 0)
            return;
        
        string provinceName = parameter[0];
        string cityName = parameter[1];
        
        ICollection<AreaModel> counties = areaService.GetCounties(provinceName, cityName);
        foreach (AreaModel district in counties)
            Counties.Add(district);
    }
    
    [RelayCommand]
    private async Task CopyResult()
    {
        if (string.IsNullOrEmpty(ResultJson))
            return;
        
        try
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime { MainWindow.Clipboard: { } clipboard })
            {
                await clipboard.SetTextAsync(ResultJson);
            }
            _ = dialogService.ShowWindowAsync(new MessageBoxViewModel
            {
                Message = "已复制到剪贴板",
            },2);
        }
        catch (Exception e)
        {
            _ = dialogService.ShowWindowAsync(new MessageBoxViewModel
            {
                Message = $"复制失败: {e.Message}",
            },2);
        }
    }
    
    [RelayCommand]
    private void ClearResult()
        => ResultJson = string.Empty;

    [RelayCommand]
    private void GenerationResult()
    {
        if (EndAge - StartAge < 0)
        {
            _ = dialogService.ShowWindowAsync(new MessageBoxViewModel
            {
                Message = "年龄范围不正确",
            },2);
            return;
        }

        List<string> birthdays = [];
        List<string> locations = [];
        List<string> locationCodes = [];
        for (int i = 0; i < GenerationCount; i++)
        {
            string birthday = GetBirthDay(CurrentBirthDateOptionType);
            if (!DateTime.TryParse(birthday, out _))
            {
                dialogService.ShowWindowAsync(new MessageBoxViewModel
                {
                    Message = "日期格式错误",
                },2);
                return;
            }
            
            locations.Add(GetLocation());
            locationCodes.Add(GetLocationCode());
            birthdays.Add(birthday);
        }

        GenerationOptions options = new GenerationOptions
        {
            Locations = locations,
            LocationCodes = locationCodes,
            BirthDays = birthdays,
            GenerationCount = GenerationCount,
            NameGenerationOptionEnum = CurrentNameType,
            GenderOptionEnum = CurrentGenderOption
        };

        ResultJson = generationService.Generate(options);
    }
    #endregion
    
    #region 私有方法

    /// <summary>
    /// 获取城市代码
    /// </summary>
    /// <returns>城市代码</returns>
    private string GetLocationCode()
        => SelectedDistrictIndex <= 0 ? _code: Counties[SelectedDistrictIndex].Code;

    /// <summary>
    /// 获取城市具体地址。
    /// 如果用户未完整选择省市区，则会进行随机选择。
    /// </summary>
    /// <returns>格式为 "省 市 县/区" 的地址字符串。</returns>
    private string GetLocation()
    {
        string provinceName;
        string cityName;
        string countyName;

        if (SelectedProvinceIndex <= 0)
        {
            int provinceIndex = Random.Next(1, Provinces.Count);
            provinceName = Provinces[provinceIndex].Name;

            var cities = areaService.GetCities(provinceName).ToList();
            int cityIndex = Random.Next(0, cities.Count);
            cityName = cities[cityIndex].Name;

            var counties = areaService.GetCounties(provinceName, cityName).ToList();
            int countyIndex = Random.Next(0, counties.Count);
            countyName = counties[countyIndex].Name;
            _code = counties[countyIndex].Code;
        }
        else
        {
            provinceName = Provinces[SelectedProvinceIndex].Name;

            if (SelectedCityIndex <= 0)
            {
                var cities = areaService.GetCities(provinceName).ToList();

                int cityIndex = Random.Next(1, cities.Count);
                cityName = cities[cityIndex].Name;

                var counties = areaService.GetCounties(provinceName, cityName).ToList();

                int countyIndex = Random.Next(1, counties.Count);
                countyName = counties[countyIndex].Name;
                _code = counties[countyIndex].Code;
            }
            else
            {
                cityName = Cities[SelectedCityIndex].Name;

                if (SelectedDistrictIndex <= 0)
                {
                    var counties = areaService.GetCounties(provinceName, cityName).ToList();

                    int countyIndex = Random.Next(1, counties.Count);
                    countyName = counties[countyIndex].Name;
                    _code = counties[countyIndex].Code;
                }
                else
                    countyName = Counties[SelectedDistrictIndex].Name;
            }
        }

        if (cityName == "县")
            cityName = string.Empty;
        return $"{provinceName} {cityName} {countyName}";
    }
    
    /// <summary>
    /// 根据指定的选项生成一个格式化的出生日期字符串。
    /// </summary>
    /// <param name="birthDateOption">用于确定如何生成出生日期的选项。</param>
    /// <returns>一个格式为 "yyyyMMdd" 的出生日期字符串，如果解析失败则可能返回空字符串或格式不正确的字符串。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当提供了不支持的 birthDateOption 时抛出。</exception>
    private string GetBirthDay(BirthDateOptionEnum birthDateOption)
    {
        switch (birthDateOption)
        {
            case BirthDateOptionEnum.BySpecificDate:
                return BirthDate;
            case BirthDateOptionEnum.ByAge:
            {
                int age = Random.Next(StartAge, EndAge + 1);
                DateTime dateTime = DateTime.Now.AddYears(-age);
                return dateTime.ToString("yyyy/MM/dd");
            }
            case BirthDateOptionEnum.Random:
            {
                int year = Random.Next(DateTime.Now.Year - 100, DateTime.Now.Year+1);
                int month = Random.Next(1, 13);
                int day = DateTime.DaysInMonth(year, month);
                
                DateTime dateTime = new DateTime(year, month, day);
                return dateTime.ToString("yyyy/MM/dd");
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(birthDateOption), "提供了无效的出生日期选项。");
        }
    }
    #endregion
}