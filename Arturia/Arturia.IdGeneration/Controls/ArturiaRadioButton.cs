using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Arturia.IdGeneration.Controls;

public class ArturiaRadioButton : RadioButton
{
    static ArturiaRadioButton()
    {
        BorderBrushProperty.OverrideDefaultValue<ArturiaRadioButton>(new SolidColorBrush(Color.Parse("#B5B5B5")));
    }
    
    public static readonly StyledProperty<double> OuterBorderSizeProperty = AvaloniaProperty.Register<ArturiaRadioButton, double>(
        nameof(OuterBorderSize), defaultValue: 26);
    public double OuterBorderSize
    {
        get => GetValue(OuterBorderSizeProperty);
        set => SetValue(OuterBorderSizeProperty, value);
    }
    
    public static readonly StyledProperty<double> InnerBorderSizeProperty = AvaloniaProperty.Register<ArturiaRadioButton, double>(
        nameof(InnerBorderSize), defaultValue: 12);
    public double InnerBorderSize
    {
        get => GetValue(InnerBorderSizeProperty);
        set => SetValue(InnerBorderSizeProperty, value);
    }

    public static readonly StyledProperty<double> StrokeThicknessProperty = AvaloniaProperty.Register<ArturiaRadioButton, double>(
        nameof(StrokeThickness), defaultValue: 2);
    public double StrokeThickness
    {
        get => GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    public static readonly StyledProperty<IBrush?> OuterStrokeProperty = AvaloniaProperty.Register<ArturiaRadioButton, IBrush?>(
        nameof(OuterStroke), new SolidColorBrush(Color.Parse("#B5B5B5")));
    public IBrush? OuterStroke
    {
        get => GetValue(OuterStrokeProperty);
        set => SetValue(OuterStrokeProperty, value);
    }
    
    public static readonly StyledProperty<IBrush?> InnerStrokeProperty = AvaloniaProperty.Register<ArturiaRadioButton, IBrush?>(
        nameof(InnerStroke), new SolidColorBrush(Color.Parse("#B5B5B5")));
    public IBrush? InnerStroke
    {
        get => GetValue(InnerStrokeProperty);
        set => SetValue(InnerStrokeProperty, value);
    }

    public static readonly StyledProperty<string?> TextProperty = AvaloniaProperty.Register<ArturiaRadioButton, string?>(
        nameof(Text), defaultValue: string.Empty);

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
}