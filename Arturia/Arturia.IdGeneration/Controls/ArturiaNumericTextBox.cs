using System.Data;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Arturia.IdGeneration.Controls;

public class ArturiaNumericTextBox : TemplatedControl
{
    private TextBox? _textBox;
    private bool _isUpdating;
    
    static ArturiaNumericTextBox()
    {
        HeightProperty.OverrideDefaultValue<ArturiaNumericTextBox>(55);
        WidthProperty.OverrideDefaultValue<ArturiaNumericTextBox>(250);
    }

    public static readonly StyledProperty<string?> TextProperty = AvaloniaProperty.Register<ArturiaNumericTextBox, string?>(
        nameof(Text), defaultValue: string.Empty);
    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly StyledProperty<string?> WatermarkProperty =
        AvaloniaProperty.Register<ArturiaNumericTextBox, string?>(
            nameof(Watermark), defaultValue: string.Empty);
    public string? Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }

    public static readonly StyledProperty<string> PatternProperty = AvaloniaProperty.Register<ArturiaNumericTextBox, string>(
        nameof(Pattern), defaultValue: "^[0-9]$");
    public string Pattern
    {
        get => GetValue(PatternProperty);
        set => SetValue(PatternProperty, value);
    }

    public static readonly StyledProperty<int> MaxValueProperty = AvaloniaProperty.Register<ArturiaNumericTextBox, int>(
        nameof(MaxValue), defaultValue: 1000);
    public int MaxValue
    {
        get => GetValue(MaxValueProperty);
        set => SetValue(MaxValueProperty, value);
    }

    public static readonly StyledProperty<int> MiniValueProperty = AvaloniaProperty.Register<ArturiaNumericTextBox, int>(
        nameof(MiniValue), defaultValue: 1);
    public int MiniValue
    {
        get => GetValue(MiniValueProperty);
        set => SetValue(MiniValueProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _textBox = e.NameScope.Find<TextBox>("PART_TextBox");
        if(_textBox is null)
            throw new InvalidExpressionException("PART_TextBox not found in template.");

        _textBox.RemoveHandler(TextInputEvent, PreviewTextInput);
        _textBox.RemoveHandler(TextBox.TextChangedEvent,TextChanged);
        
        _textBox.AddHandler(TextInputEvent, PreviewTextInput, RoutingStrategies.Tunnel);
        _textBox.AddHandler(TextBox.TextChangedEvent, TextChanged, RoutingStrategies.Bubble);
    }

    private void PreviewTextInput(object? sender, TextInputEventArgs e)
    {
        if (_textBox is null || string.IsNullOrEmpty(e.Text))
            return;
        
        if (!Regex.IsMatch(e.Text,Pattern))
            e.Handled = true;
    }

    private void TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (_textBox is null || _isUpdating)
            return;

        _isUpdating = true;

        string currentText = _textBox.Text ?? string.Empty;
        string digitsOnly = Regex.Replace(currentText, "[^0-9]", string.Empty);

        if (int.TryParse(digitsOnly, out var value))
        {
            if(value > MaxValue)
                digitsOnly = MaxValue.ToString();
            else if (value < MiniValue)
                digitsOnly = MiniValue.ToString();
        }

        if (!currentText.Equals(digitsOnly))
        {
            _textBox.Text = digitsOnly;
            _textBox.CaretIndex = digitsOnly.Length;
        }

        Text = _textBox.Text;
        _isUpdating = false;
    }
}