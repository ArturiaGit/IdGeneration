using System;
using System.Collections.Generic;
using System.Linq;
using Arturia.IdGeneration.Models;
using Microsoft.Extensions.Options;

namespace Arturia.IdGeneration.Services;

public class AreaService(IOptions<List<AreaModel>> areas) : IAreaService
{
    private readonly IReadOnlyCollection<AreaModel> _areas = areas.Value;


    public ICollection<AreaModel> GetProvinces()
        => _areas.Select(r => r).ToList();

    public ICollection<AreaModel> GetCities(string provinceName)
        => _areas.Where(r => r.Name.Equals(provinceName)).SelectMany(r =>
        {
            if (r.Children != null)
                return r.Children;
            return new List<AreaModel>();
        }).Select(r => r).ToList();

    public ICollection<AreaModel> GetDistricts(string provinceName, string cityName)
    {
        ICollection<AreaModel> cityes = _areas.Where(r=>r.Name.Equals(provinceName))
            .SelectMany(r => r.Children!)
            .ToList();
        if (cityes.Count <= 0)
            return new List<AreaModel>();

        return cityes.Where(r=>r.Name.Equals(cityName))
            .SelectMany(r => r.Children!)
            .Select(r=>r)
            .ToList();
    }
}