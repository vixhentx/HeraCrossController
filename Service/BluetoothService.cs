using HeraCrossController.Model;
using Plugin.BluetoothClassic.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeraCrossController.Service
{
    public class BluetoothService
    {
        private IBluetoothAdapter _adapter = DependencyService.Resolve<IBluetoothAdapter>();
        private IBluetoothConnection? _connection;

        public ConnectionStatusEnum ConnectionStatus { get; private set; }
        public BluetoothService()
        {
            ConnectionStatus = ConnectionStatusEnum.Disconnected;
        }
        public IEnumerable<BluetoothDeviceModel> ScanForDevices()
        {
            if(!_adapter.Enabled) _adapter.Enable();
            _adapter.StartDiscovery();
            return _adapter.BondedDevices;
        }
        public void Connect(BluetoothDeviceModel device)
        {
            if (!_adapter.Enabled) _adapter.Enable();
            _connection = _adapter.CreateConnection(device);
            ConnectionStatus = _connection == null ? ConnectionStatusEnum.Disconnected : ConnectionStatusEnum.Connected;
        }
        public void SendData(byte[] data,int len)
        {
            Memory<byte> mem = new(data, 0, len);
            _connection.RetryTransmitAsync(mem);
        }
    }
}
