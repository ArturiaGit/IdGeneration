using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Arturia.IdGeneration.Views;

public partial class DisclaimerView : Window
{
    public DisclaimerView()
    {
        InitializeComponent();
    }


    private void Button_Close(object? sender, RoutedEventArgs e) => Close(false);

    private void Button_Accept(object? sender, RoutedEventArgs e) => Close(true);
}