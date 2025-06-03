using HeraCrossController.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeraCrossController.Interfaces
{
    public interface IDialogService
    {
        public Task<BluetoothSerialDevice?> ShowDevicesAsync(List<BluetoothSerialDevice> devices,string title="请选择蓝牙设备",string confirm="OK");
        public void ShowMessageBox(string message,string title="提示",string confirm="OK");
    }
}
