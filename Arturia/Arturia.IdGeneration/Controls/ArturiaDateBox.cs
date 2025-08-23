using System;
using System.Data;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace Arturia.IdGeneration.Controls;

public class ArturiaDateBox : TemplatedControl
{
    private TextBox? _textBox;
    private bool _isFormatting;
    
    static ArturiaDateBox()
    {
        HeightProperty.OverrideDefaultValue<ArturiaDateBox>(55);
        WidthProperty.OverrideDefaultValue<ArturiaDateBox>(250);
    }
    
    public static readonly StyledProperty<double> IconHeightProperty = AvaloniaProperty.Register<ArturiaDateBox, double>(
        nameof(IconHeight), defaultValue: 20.0);
    public double IconHeight
    {
        get => GetValue(IconHeightProperty);
        set => SetValue(IconHeightProperty, value);
    }

    public static readonly StyledProperty<double> IconWidthProperty = AvaloniaProperty.Register<ArturiaDateBox, double>(
        nameof(IconWidth), defaultValue: 20.0);
    public double IconWidth
    {
        get => GetValue(IconWidthProperty);
        set => SetValue(IconWidthProperty, value);
    }

    public static readonly StyledProperty<Geometry?> IconDataProperty = AvaloniaProperty.Register<ArturiaDateBox, Geometry?>(
        nameof(IconData), defaultValue: Geometry.Parse("M128 64s0.512 0 0 0m64 745.984H384v-128H192v128M832 0H768v192h64V0M256 0H192v192H256V0M192 617.984H384v-128H192v128M895.488 64s0.512 0 0 0m-191.488 553.984h128v-128h-128v128M896 64v96.256c0 53.248-43.008 95.744-96.256 95.744s-95.744-43.008-95.744-96.256V63.488h-384v96.256c0 53.248-43.008 95.744-96.256 95.744C171.008 256 128 212.992 128 159.744V64c-70.656 0.512-128 57.856-128 128.512v702.464C0 965.632 57.344 1024 128.512 1024h766.976c71.168 0 128.512-57.344 128.512-128.512V192.512c0-70.656-57.344-128-128-128.512m62.976 833.024c0 35.328-28.672 64-64 64h-768c-35.328 0-64-28.672-64-64V427.52c0-35.328 28.672-64 64-64h768c35.328 0 64 28.672 64 64v469.504m-510.976-279.04H640v-128H448v128m0 192H640v-128H448v128m256 0h128v-128h-128v128m0 0z"));
    public Geometry? IconData
    {
        get => GetValue(IconDataProperty);
        set => SetValue(IconDataProperty, value);
    }

    public static readonly StyledProperty<IBrush?> IconForegroundProperty = AvaloniaProperty.Register<ArturiaDateBox, IBrush?>(
        nameof(IconForeground), defaultValue: new SolidColorBrush(Color.Parse("#CECECE")));
    public IBrush? IconForeground
    {
        get => GetValue(IconForegroundProperty);
        set => SetValue(IconForegroundProperty, value);
    }

    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<ArturiaDateBox, string?>(
            nameof(Text), defaultValue: string.Empty);
    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly StyledProperty<string?> WatermarkProperty = AvaloniaProperty.Register<ArturiaDateBox, string?>(
        nameof(Watermark), defaultValue: string.Empty);
    public string? Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        
        _textBox = e.NameScope.Find<TextBox>("PART_TextBox");
        if(_textBox is null)
            throw new InvalidExpressionException("PART_TextBox not found in the template.");

        _textBox.RemoveHandler(TextInputEvent, PreviewTextInput);
        _textBox.RemoveHandler(TextBox.TextChangedEvent, TextChanged);
        
        _textBox.AddHandler(TextInputEvent, PreviewTextInput, RoutingStrategies.Tunnel);
        _textBox.AddHandler(TextBox.TextChangedEvent, TextChanged, RoutingStrategies.Bubble);
    }

    private void PreviewTextInput(object? sender, TextInputEventArgs e)
    {
        if (string.IsNullOrEmpty(e.Text) || _textBox is null)
            return;
        
        if(!Regex.IsMatch(e.Text,"^[0-9]$") || (_textBox.Text?.Length ?? 0) >= 10)
            e.Handled = true;
    }

    private void TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (_isFormatting || _textBox is null)
            return;

        _isFormatting = true;

        string currentText = _textBox.Text ?? string.Empty;
        string formattedText = Regex.Replace(currentText, "[^0-9]", string.Empty);

        if (formattedText.Length > 4)
            formattedText = formattedText.Insert(4, "/");
        if (formattedText.Length > 7)
            formattedText = formattedText.Insert(7, "/");
        if(formattedText.Length > 10)
            formattedText = formattedText.Substring(0,10);

        if (!currentText.Equals(formattedText))
        {
            _textBox.Text = formattedText;
            _textBox.CaretIndex = formattedText.Length;
        }

        Text = _textBox.Text;
        _isFormatting = false;
    }
}