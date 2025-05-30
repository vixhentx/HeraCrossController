using HeraCrossController.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeraCrossController.Interfaces
{
    public interface IBluetoothSerial
    {
        public const string SPP_UUID = "00001101-0000-1000-8000-00805F9B34FB";
        Task<List<BluetoothSerialDevice>> DiscoverDevicesAsync();
        Task ConnectAsync(BluetoothSerialDevice device);
        Task SendDataAsync(Memory<byte> data);
        ConnectionStatusEnum ConnectionStatus { get; }
        event EventHandler<Memory<byte>> OnDataReceived;
    }
}
