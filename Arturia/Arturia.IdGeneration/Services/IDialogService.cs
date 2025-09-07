using System.Threading.Tasks;
using Arturia.IdGeneration.ViewModels;

namespace Arturia.IdGeneration.Services;

public interface IDialogService
{
    public Task ShowWindowAsync<TViewModel>(TViewModel viewModel,int seconds) where TViewModel : ViewModelBase;
}