using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace IdGeneration.CustomControl;

public class CounterStyle : TemplatedControl
{
    private Button? _upButton;
    private Button? _downButton;
    private TextBox? _inputBox;
    
    private int _currentNumber; //计数器，用于记录上一次的数值是多少
    
    static CounterStyle()
    {
        HeightProperty.OverrideDefaultValue<CounterStyle>(48);
        WidthProperty.OverrideDefaultValue<CounterStyle>(225);
        CornerRadiusProperty.OverrideDefaultValue<CounterStyle>(new CornerRadius(5));
        BorderBrushProperty.OverrideDefaultValue<CounterStyle>(new SolidColorBrush(Color.Parse("#BEBEBE")));
        BorderThicknessProperty.OverrideDefaultValue<CounterStyle>(new Thickness(2));
        BackgroundProperty.OverrideDefaultValue<CounterStyle>(new SolidColorBrush(Color.Parse("#FFFFFF")));

        // TextProperty.Changed.AddClassHandler<CounterStyle>((x, e) => x.OnTextChanged(e));
    }

    #region 公开属性
    public static readonly StyledProperty<int> MaximumProperty =
        AvaloniaProperty.Register<CounterStyle, int>(nameof(Maximum),defaultValue:9999);
    public int Maximum
    {
        get => GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    public static readonly StyledProperty<int> MinimumProperty = AvaloniaProperty.Register<CounterStyle, int>(
        nameof(Minimum),defaultValue:0);
    public int Minimum
    {
        get => GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    public static readonly StyledProperty<int> IncrementProperty = AvaloniaProperty.Register<CounterStyle, int>(
        nameof(Increment), defaultValue: 1);
    public int Increment
    {
        get => GetValue(IncrementProperty);
        set => SetValue(IncrementProperty, value);
    }

    public static readonly StyledProperty<string?> WatermarkProperty =
        AvaloniaProperty.Register<CounterStyle, string?>(nameof(Watermark), defaultValue: string.Empty);
    public string? Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }

    public static readonly StyledProperty<string?> TextProperty = AvaloniaProperty.Register<CounterStyle, string?>(
        nameof(Text));
    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    #endregion

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        
        _upButton ??= e.NameScope.Find<Button>("PART_UpButton");
        _downButton ??= e.NameScope.Find<Button>("PART_DownButton");
        _inputBox ??=e.NameScope.Find<TextBox>("PART_InputBox");

        if (_upButton is null)
            throw new InvalidOperationException(
                "Cannot perform this operation because the 'PART_UpButton' was not found in the control's template.");
        if (_downButton is null)
            throw new InvalidOperationException(
                "Cannot perform this operation because the 'PART_DownButton' was not found in the control's template.");
        if (_inputBox is null)
            throw new InvalidOperationException(
                "Cannot perform this operation because the 'PART_InputBox' was not found in the control's template.");
        
        _upButton.Click -= UpButton_Click;
        _upButton.Click += UpButton_Click;
        
        _downButton.Click -= DownButton_Click;
        _downButton.Click += DownButton_Click;
    }

    #region 计数器
    private void UpButton_Click(object? sender, RoutedEventArgs e)
    {
        if (_inputBox is null)
            throw new InvalidOperationException(
                "Cannot perform this operation because the 'PART_InputBox' was not found in the control's template.");
        
        if (!int.TryParse(_inputBox.Text, out int inputValue))
        {
            Text = _currentNumber.ToString();
            _inputBox.Text = _currentNumber.ToString();
            
            return;
        }
        
        inputValue += Increment;
        inputValue = inputValue > Maximum ? Maximum : inputValue;
        
        _currentNumber = inputValue;
        Text = inputValue.ToString();
        _inputBox!.Text = inputValue.ToString();
    }
    
    private void DownButton_Click(object? sender, RoutedEventArgs e)
    {
        if (_inputBox is null)
            throw new InvalidOperationException(
                "Cannot perform this operation because the 'PART_InputBox' was not found in the control's template.");

        if (!int.TryParse(_inputBox.Text, out int inputValue))
        {
            Text = _currentNumber.ToString();
            _inputBox.Text = _currentNumber.ToString();
            
            return;
        }
        
        inputValue -= Increment;
        inputValue = inputValue < Minimum ? Minimum : inputValue;
        
        _currentNumber = inputValue;
        Text = inputValue.ToString();
        _inputBox.Text = inputValue.ToString();
    }
    #endregion
}