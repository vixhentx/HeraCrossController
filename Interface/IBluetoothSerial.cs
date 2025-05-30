using Android.Bluetooth;
using HeraCrossController.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeraCrossController.Interface
{
    public interface IBluetoothSerial
    {
        Task<List<BluetoothSerialDevice>> DiscoverDevicesAsync();
        Task ConnectAsync(BluetoothSerialDevice device);
        Task SendDataAsync(Memory<byte> data);
        ConnectionStatusEnum ConnectionStatus { get; }
        event EventHandler<Memory<byte>> OnDataReceived;
    }
}
