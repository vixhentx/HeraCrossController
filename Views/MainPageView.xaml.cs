using HeraCrossController.ViewModels;

namespace HeraCrossController.Views
{
    public partial class MainPageView : ContentPage
    {
        public MainPageView(MainPageViewModel viewModel)
        {
            BindingContext = viewModel;
            InitializeComponent();
        }
    }

}
