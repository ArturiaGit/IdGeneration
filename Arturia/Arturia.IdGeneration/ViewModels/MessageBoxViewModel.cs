using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Arturia.IdGeneration.ViewModels;

public partial class MessageBoxViewModel : ViewModelBase
{
    [ObservableProperty]
    private double _width = 400;
    
    [ObservableProperty]
    private double _height = 200;
    
    [ObservableProperty]
    private string _message = string.Empty;
}