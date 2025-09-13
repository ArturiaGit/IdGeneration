using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

// ReSharper disable InconsistentNaming
namespace Arturia.IdGeneration.Controls;

public class ArturiaLabel : ContentControl
{
    static ArturiaLabel()
    {
        CornerRadiusProperty.OverrideDefaultValue<ArturiaLabel>(new CornerRadius(5,0,0,5));
        BackgroundProperty.OverrideDefaultValue<ArturiaLabel>(new SolidColorBrush(Color.Parse("#ECECEC")));
        BorderThicknessProperty.OverrideDefaultValue<ArturiaLabel>(new Thickness(1));
        BorderBrushProperty.OverrideDefaultValue<ArturiaLabel>(new SolidColorBrush(Color.Parse("#B5B5B5")));
        WidthProperty.OverrideDefaultValue<ArturiaLabel>(130);
        HeightProperty.OverrideDefaultValue<ArturiaLabel>(45);
    }
    
    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<ArturiaLabel, string?>(nameof(Text),string.Empty);
    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
}