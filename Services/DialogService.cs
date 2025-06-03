using HeraCrossController.Interfaces;
using HeraCrossController.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeraCrossController.Services
{
    public class DialogService : IDialogService
    {
        static Page _mainpage = Application.Current?.MainPage ?? throw new NullReferenceException();
        public async Task<BluetoothSerialDevice?> ShowDevicesAsync(List<BluetoothSerialDevice> devices, string title = "请选择蓝牙设备", string confirm = "OK")
        {
            ViewModels.DeviceSelectViewModel vm = new()
            {
                Title = title,
                Devices = devices,
                Confirm = confirm
            };
            Views.DeviceSelectView popup = new(vm);
            await _mainpage.Navigation.PushModalAsync(popup);
            return vm.SelectedDevice;
        }

        public void ShowMessageBox(string message, string title = "提示", string confirm = "OK")
        {
            _ = _mainpage.DisplayAlert(message, title, confirm);
        }
    }
}
