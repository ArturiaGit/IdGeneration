using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Media;

namespace IdGeneration.CustomControl;

public class LabelControl : ContentControl
{
    public new static readonly StyledProperty<IBrush> BackgroundProperty = AvaloniaProperty.Register<LabelControl, IBrush>(
        nameof(Background), defaultValue: new SolidColorBrush(Color.Parse("#F6F6F6")));
    public new IBrush Background
    {
        get => GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    public new static readonly StyledProperty<IBrush> BorderBrushProperty = AvaloniaProperty.Register<LabelControl, IBrush>(
        nameof(BorderBrush), defaultValue: new SolidColorBrush(Color.Parse("#E3E3E3")));
    public new IBrush BorderBrush
    {
        get => GetValue(BorderBrushProperty);
        set => SetValue(BorderBrushProperty, value);
    }
    
    public new static readonly StyledProperty<Thickness> BorderThicknessProperty = AvaloniaProperty.Register<LabelControl, Thickness>(
        nameof(BorderThickness), defaultValue: new Thickness(1));
    public new Thickness BorderThickness
    {
        get => GetValue(BorderThicknessProperty);
        set => SetValue(BorderThicknessProperty, value);
    }

    public new static readonly StyledProperty<CornerRadius> CornerRadiusProperty = AvaloniaProperty.Register<LabelControl, CornerRadius>(
        nameof(CornerRadius), defaultValue: new CornerRadius(5,0,0,5));
    public new CornerRadius CornerRadius
    {
        get => GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public new static readonly StyledProperty<double> HeightProperty = AvaloniaProperty.Register<LabelControl, double>(
        nameof(Height), defaultValue: 48);
    public new double Height
    {
        get => GetValue(HeightProperty);
        set => SetValue(HeightProperty, value);
    }
    
    public new static readonly StyledProperty<double> WidthProperty = AvaloniaProperty.Register<LabelControl, double>(
        nameof(Width), defaultValue: 127);
    public new double Width
    {
        get => GetValue(WidthProperty);
        set => SetValue(WidthProperty, value);
    }
    
    public static readonly StyledProperty<string?> TextProperty = AvaloniaProperty.Register<LabelControl, string?>(
        nameof(Text), defaultValue: string.Empty);
    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
}