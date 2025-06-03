using HeraCrossController.Models;
using HeraCrossController.ViewModels;

namespace HeraCrossController.Views;

public partial class DeviceSelectView : ContentPage
{
	private DeviceSelectViewModel vm;
    public DeviceSelectView(DeviceSelectViewModel viewModel)
	{
		BindingContext = viewModel;
		vm = viewModel;
		InitializeComponent();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
		vm.SelectionTask.TrySetResult(vm.SelectedDevice);
		await Navigation.PopModalAsync();
    }
}