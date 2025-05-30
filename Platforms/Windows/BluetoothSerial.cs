using HeraCrossController.Interfaces;
using HeraCrossController.Models;
using InTheHand.Net;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HeraCrossController.Platforms.Windows
{
    //异常都在外面Catch
    public class BluetoothSerial : IBluetoothSerial
    {
        private BluetoothClient? _bluetoothClient;
        private NetworkStream? _networkStream;
        private CancellationTokenSource? _receiveCancellationTokenSource;
        private BluetoothDeviceInfo? _connectedDevice;
        private ConnectionStatusEnum _connectionStatus = ConnectionStatusEnum.Disconnected;
        public ConnectionStatusEnum ConnectionStatus
        {
            get => _connectionStatus;
            set
            {
                _connectionStatus = value;
                OnConnectionStatusChanged?.Invoke(this, value);
            }
        }

        public event EventHandler<Memory<byte>>? OnDataReceived;
        public event EventHandler<ConnectionStatusEnum>? OnConnectionStatusChanged;

        public async Task ConnectAsync(BluetoothSerialDevice device)
        {
            if (ConnectionStatus == ConnectionStatusEnum.Connected)
                await DisconnectAsync();

            ConnectionStatus = ConnectionStatusEnum.Connecting;

            _bluetoothClient = new BluetoothClient();
            var address = BluetoothAddress.Parse(device.Address);
            var serviceGuid = new Guid(IBluetoothSerial.SPP_UUID);

            Debug.WriteLine("尝试连接");
            await Task.Run(() => _bluetoothClient.Connect(address, serviceGuid));

            if (!_bluetoothClient.Connected)
            {
                ConnectionStatus = ConnectionStatusEnum.Disconnected;
                throw new Exception("连接失败!设备可能不支持SPP!");
            }

            _networkStream = _bluetoothClient.GetStream();
            _connectedDevice = new BluetoothDeviceInfo(address);
            ConnectionStatus = ConnectionStatusEnum.Connected;

            _receiveCancellationTokenSource = new CancellationTokenSource();
            _ = Task.Run(() => ReceiveDataAsync(_receiveCancellationTokenSource.Token));
        }


        public async Task DisconnectAsync()
        {
            _receiveCancellationTokenSource?.Cancel();

            if (_networkStream != null)
            {
                await _networkStream.FlushAsync();
                _networkStream.Close();
                _networkStream = null;
            }

            if (_bluetoothClient != null)
            {
                _bluetoothClient.Close();
                _bluetoothClient.Dispose();
                _bluetoothClient = null;
            }

            ConnectionStatus = ConnectionStatusEnum.Disconnected;
            _connectedDevice = null;

        }

        public async Task<List<BluetoothSerialDevice>?> DiscoverDevicesAsync()
        {
            using var t_client = new BluetoothClient();
            var last_stat = ConnectionStatus;
            ConnectionStatus = ConnectionStatusEnum.Discovering;
            var devices = await Task.Run(() => t_client.DiscoverDevices(
                maxDevices: 6
            ));

            ConnectionStatus = last_stat;
            return devices
                .Where(d => d.DeviceName != null)
                .Select(d => BluetoothSerialDevice.Create(d.DeviceName,d.DeviceAddress.ToString()))
                .ToList();
        }


        public async Task SendDataAsync(Memory<byte> data)
        {
            if (ConnectionStatus != ConnectionStatusEnum.Connected || _networkStream == null)
                throw new InvalidOperationException("未连接!");

            await _networkStream.WriteAsync(data.ToArray(), 0, data.Length);
            await _networkStream.FlushAsync();
        }
        private async void ReceiveDataAsync(CancellationToken token)
        {
            var buffer = new byte[1024];

            try
            {
                while (!token.IsCancellationRequested &&
                       _networkStream != null &&
                       _bluetoothClient != null &&
                       _bluetoothClient.Connected)
                {
                    var bytesRead = await _networkStream.ReadAsync(buffer, token);

                    if (bytesRead == 0)
                    {
                        await DisconnectAsync();
                        return;
                    }

                    OnDataReceived?.Invoke(this, new Memory<byte>(buffer, 0, bytesRead));
                }
            }
            catch (OperationCanceledException)
            {
                await DisconnectAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"接收数据出错: {ex.Message}");
                await DisconnectAsync();
                ConnectionStatus = ConnectionStatusEnum.Error;
            }
        }
    }
}
