using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace Arturia.IdGeneration.Converters;

public class EnumToBooleanConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value?.Equals(parameter);

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value?.Equals(true) == true ? parameter : AvaloniaProperty.UnsetValue;
}