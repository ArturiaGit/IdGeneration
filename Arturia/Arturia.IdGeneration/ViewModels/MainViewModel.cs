using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Arturia.IdGeneration.Enums;
using Arturia.IdGeneration.Models;
using Arturia.IdGeneration.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Arturia.IdGeneration.ViewModels;

public partial class MainViewModel(IAreaService areaService, IDialogService dialogService,IGenerationService generationService) : ViewModelBase
{
    #region 私有字段
    private static readonly Random Random = new();
    
    private readonly int[] _maleDigits = { 1, 3, 5, 7, 9 };
    private readonly int[] _femaleDigits = { 0, 2, 4, 6, 8 };

    private bool _isUpdatingAges = false;
    #endregion
    
    #region 通知属性
    [ObservableProperty]
    private ObservableCollection<AreaModel> _provinces =  new();
    
    [ObservableProperty]
    private ObservableCollection<AreaModel> _cities = new();
    
    [ObservableProperty]
    private ObservableCollection<AreaModel> _districts = new();
    
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
    private GenderOptionEnum _currentGenderOption = GenderOptionEnum.Male;
    
    [ObservableProperty]
    private BirthDateOptionEnum _currentBirthDateOptionType = BirthDateOptionEnum.BySpecificDate;
    
    [ObservableProperty]
    private NameGenerationOptionEnum _currentNameType = NameGenerationOptionEnum.None;
    #endregion 通知属性
    
    #region 命令
    [RelayCommand]
    private void LoadProvinceNames()
    {
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
    private void LoadCityNames(AreaModel? province)
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
    private void LoadDistrictNames(List<string> parameter)
    {
        Districts.Clear();
        Districts.Insert(0,new  AreaModel
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
        
        ICollection<AreaModel> districts = areaService.GetDistricts(provinceName, cityName);
        foreach (AreaModel district in districts)
            Districts.Add(district);
    }

    [RelayCommand]
    private void GenerationResult()
    {
        string locationCode = GetLocationCode();
        if (string.IsNullOrEmpty(locationCode))
        {
            dialogService.ShowWindow(new MessageBoxViewModel
            {
                Message = "请选择出生地址",
            },2);
            return;
        }

        if (EndAge - StartAge < 0)
        {
            dialogService.ShowWindow(new MessageBoxViewModel
            {
                Message = "年龄范围不正确",
            },2);
            return;
        }

        List<string> birthdays = [];
        for (int i = 0; i < GenerationCount; i++)
        {
            string birthday = GetBirthDay(CurrentBirthDateOptionType);
            if (!DateTime.TryParse(birthday, out _))
            {
                dialogService.ShowWindow(new MessageBoxViewModel
                {
                    Message = "日期格式错误",
                },2);
                return;
            }
            
            birthdays.Add(birthday);
        }

        GenerationOptions options = new GenerationOptions
        {
            Location = GetLocation(),
            LocationCode = locationCode,
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
    {
        if (SelectedDistrictIndex <= 0)
            return string.Empty;

        return Districts[SelectedDistrictIndex].Code;
    }

    /// <summary>
    /// 获取城市具体地址
    /// </summary>
    /// <returns>城市具体地址</returns>
    private string GetLocation()
    {
        if (SelectedProvinceIndex <= 0 || SelectedCityIndex <= 0 || SelectedDistrictIndex <= 0)
            return string.Empty;

        return
            $"{Provinces[SelectedProvinceIndex].Name} {Cities[SelectedCityIndex].Name} {Districts[SelectedDistrictIndex].Name}";
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