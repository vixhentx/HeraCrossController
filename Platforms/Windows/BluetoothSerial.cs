using HeraCrossController.Interfaces;
using HeraCrossController.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeraCrossController.Platforms.Windows
{
    public class BluetoothSerial : IBluetoothSerial
    {
        public ConnectionStatusEnum ConnectionStatus { get; private set; }

        public event EventHandler<Memory<byte>>? OnDataReceived;

        public Task ConnectAsync(BluetoothSerialDevice device)
        {
            throw new NotImplementedException();
        }

        public Task<List<BluetoothSerialDevice>> DiscoverDevicesAsync()
        {
            throw new NotImplementedException();
        }

        public Task SendDataAsync(Memory<byte> data)
        {
            throw new NotImplementedException();
        }
    }
}
