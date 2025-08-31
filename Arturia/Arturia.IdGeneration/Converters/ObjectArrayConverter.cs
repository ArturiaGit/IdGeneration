using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Arturia.IdGeneration.Models;
using Avalonia.Data.Converters;

namespace Arturia.IdGeneration.Converters;

public class ObjectArrayConverter : IMultiValueConverter
{
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        List<string> names = new();
        if (values.Count < 0)
            return names;

        foreach (var value in values)
            if (value is AreaModel area)
                names.Add(area.Name);

        return names;
    }
}