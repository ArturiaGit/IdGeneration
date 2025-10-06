using System.Collections.Generic;
using System.Collections.ObjectModel;
using Arturia.IdGeneration.Models;

namespace Arturia.IdGeneration.Services;

public interface IAreaService
{
    /// <summary>
    /// 获取省份集合
    /// </summary>
    /// <returns>省份名称集合</returns>
    public ICollection<AreaModel> GetProvinces();
    
    /// <summary>
    /// 根据省份名称获取城市集合
    /// </summary>
    /// <param name="provinceName">省份名称</param>
    /// <returns>城市集合</returns>
    public ICollection<AreaModel> GetCities(string provinceName);
    
    /// <summary>
    /// 根据省份名称和城市名称获取区县集合
    /// </summary>
    /// <param name="provinceName">省份名称</param>
    /// <param name="cityName">市名称</param>
    /// <returns>县区名称集合</returns>
    public ICollection<AreaModel> GetCounties(string provinceName,string cityName);
}