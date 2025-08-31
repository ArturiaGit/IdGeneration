using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Arturia.IdGeneration.Converters;

public class StringToIntConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int)
            return value.ToString();

        throw new FormatException("无法从 Int类型 转换为 String类型");
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string stringValue)
        {
            if (int.TryParse(stringValue, out var result))
                return result;
        }

        throw new FormatException("无法从 String类型 转换为 Int类型");
    }
}