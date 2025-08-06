using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;

namespace IdGeneration.CustomControl;

public class RadioButtonControl : TemplatedControl
{
    public static readonly StyledProperty<string> TextProperty = AvaloniaProperty.Register<RadioButtonControl, string>(
        nameof(Text), defaultValue: string.Empty);
    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly StyledProperty<string> GroupNameProperty = AvaloniaProperty.Register<RadioButtonControl, string>(
        nameof(GroupName), defaultValue: string.Empty);
    public string GroupName
    {
        get => GetValue(GroupNameProperty);
        set => SetValue(GroupNameProperty, value);
    }

    public static readonly StyledProperty<bool?> IsCheckedProperty = AvaloniaProperty.Register<RadioButtonControl, bool?>(
        nameof(IsChecked), defaultValue: false);
    public bool? IsChecked
    {
        get => GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }
}