using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Arturia.IdGeneration.Controls;

public class ArturiaRadioButton : RadioButton
{
    static ArturiaRadioButton()
    {
        BorderBrushProperty.OverrideDefaultValue<ArturiaRadioButton>(new SolidColorBrush(Color.Parse("#B5B5B5")));
        BorderThicknessProperty.OverrideDefaultValue<ArturiaRadioButton>(new Thickness(2));
    }
    
    public static readonly StyledProperty<double> OuterBorderSizeProperty = AvaloniaProperty.Register<ArturiaRadioButton, double>(
        nameof(OuterBorderSize), defaultValue: 20);
    public double OuterBorderSize
    {
        get => GetValue(OuterBorderSizeProperty);
        set => SetValue(OuterBorderSizeProperty, value);
    }
    
    public static readonly StyledProperty<double> InnerBorderSizeProperty = AvaloniaProperty.Register<ArturiaRadioButton, double>(
        nameof(InnerBorderSize), defaultValue: 8);
    public double InnerBorderSize
    {
        get => GetValue(InnerBorderSizeProperty);
        set => SetValue(InnerBorderSizeProperty, value);
    }
    
    public static readonly StyledProperty<IBrush?> OuterBorderProperty = AvaloniaProperty.Register<ArturiaRadioButton, IBrush?>(
        nameof(OuterBorder), new SolidColorBrush(Color.Parse("#B5B5B5")));
    public IBrush? OuterBorder
    {
        get => GetValue(OuterBorderProperty);
        set => SetValue(OuterBorderProperty, value);
    }
    
    public static readonly StyledProperty<IBrush?> InnerBorderProperty = AvaloniaProperty.Register<ArturiaRadioButton, IBrush?>(
        nameof(InnerBorder), new SolidColorBrush(Color.Parse("#B5B5B5")));
    public IBrush? InnerBorder
    {
        get => GetValue(InnerBorderProperty);
        set => SetValue(InnerBorderProperty, value);
    }

    public static readonly StyledProperty<string?> TextProperty = AvaloniaProperty.Register<ArturiaRadioButton, string?>(
        nameof(Text), defaultValue: string.Empty);

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
}