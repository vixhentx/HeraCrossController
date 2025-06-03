using HeraCrossController.Models;
using HeraCrossController.ViewModels;

namespace HeraCrossController.Views;

public partial class DeviceSelectView : ContentPage
{
	
	public DeviceSelectView(DeviceSelectViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
		await Navigation.PopModalAsync();
    }
}