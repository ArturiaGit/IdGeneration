using System.Threading.Tasks;
using Arturia.IdGeneration.ViewModels;

namespace Arturia.IdGeneration.Services;

public interface IDialogService
{
    public Task<object> ShowWindowAsync<TViewModel>(TViewModel viewModel,int seconds=0) where TViewModel : ViewModelBase;
}