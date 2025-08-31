using Arturia.IdGeneration.ViewModels;

namespace Arturia.IdGeneration.Services;

public interface IDialogService
{
    public void ShowWindow<TViewModel>(TViewModel viewModel,int seconds) where TViewModel : ViewModelBase;
}