using HeraCrossController.Models;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HeraCrossController.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class DeviceSelectViewModel
    {
        public required List<BluetoothSerialDevice> Devices { get; set; }
        public required string Title { get; set; }
        public required string Confirm { get; set; }
        public BluetoothSerialDevice? SelectedDevice { get; set; }

        public TaskCompletionSource<BluetoothSerialDevice?> SelectionTask { get; } = new();

    }
}
