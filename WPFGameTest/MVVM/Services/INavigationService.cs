using WPFGameTest.MVVM.ViewModel;

namespace WPFGameTest.MVVM.Services
{
    public interface INavigationService<TViewModel> where TViewModel:ViewModelBase
    {
        void Navigate();
    }
}