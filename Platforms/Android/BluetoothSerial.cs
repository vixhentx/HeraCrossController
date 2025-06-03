using Android.Bluetooth;
using Android.Content;
using HeraCrossController.Interfaces;
using HeraCrossController.Models;
using Java.IO;
using Java.Util;
using System.Collections.Concurrent;

namespace HeraCrossController.Platforms.Android
{
    public class BluetoothSerial : IBluetoothSerial
    {
        private readonly UUID SPP_UUID = UUID.FromString(IBluetoothSerial.SPP_UUID);
        private ConnectionStatusEnum _connectionStatus = ConnectionStatusEnum.Disconnected;
        private readonly BluetoothAdapter _adapter = BluetoothAdapter.DefaultAdapter ?? throw new InvalidOperationException("该设备不支持蓝牙");
        private Context _context = Platform.AppContext;
        private BluetoothSocket? _socket;
        private Stream? _inputStream;
        private Stream? _outputStream;
        private CancellationTokenSource? _readCancellationToken;
        public ConnectionStatusEnum ConnectionStatus
        {
            get => _connectionStatus;
            private set
            {
                _connectionStatus = value;
                OnConnectionStatusChanged?.Invoke(this, value);
            }
        }

        public event EventHandler<Memory<byte>>? OnDataReceived;
        public event EventHandler<ConnectionStatusEnum>? OnConnectionStatusChanged;

        public async Task ConnectAsync(BluetoothSerialDevice device)
        {
            await EnsureBluetoothReady();
            if (ConnectionStatus == ConnectionStatusEnum.Connected)
                await DisconnectAsync();

            ConnectionStatus = ConnectionStatusEnum.Connecting;

            try
            {
                var nativeDevice = _adapter.GetRemoteDevice(device.Address);
                _socket = nativeDevice.CreateRfcommSocketToServiceRecord(SPP_UUID);

                await _socket.ConnectAsync();
                ConnectionStatus = ConnectionStatusEnum.Connected;

                _inputStream = _socket.InputStream;
                _outputStream = _socket.OutputStream;

                // 开始监听数据
                _readCancellationToken = new CancellationTokenSource();
                Task.Run(() => ListenForData(_readCancellationToken.Token));
            }
            catch
            {
                ConnectionStatus = ConnectionStatusEnum.Disconnected;
                throw;
            }
        }

        public async Task DisconnectAsync()
        {
            if (ConnectionStatus != ConnectionStatusEnum.Connected)
                return;

            ConnectionStatus = ConnectionStatusEnum.Disconnecting;

            try
            {
                _readCancellationToken?.Cancel();
                _socket?.Close();
                _socket?.Dispose();
                _inputStream?.Dispose();
                _outputStream?.Dispose();
            }
            finally
            {
                _socket = null;
                _inputStream = null;
                _outputStream = null;
                ConnectionStatus = ConnectionStatusEnum.Disconnected;
            }
        }

        public async Task<List<BluetoothSerialDevice>?> DiscoverDevicesAsync()
        {
            await EnsureBluetoothReady();

            if (_adapter.IsDiscovering)
                _adapter.CancelDiscovery();



            List<BluetoothSerialDevice> devices = [];
            TaskCompletionSource<bool> discoveryCompletion = new();
            BluetoothReceiver receiver = new(discoveryCompletion);

            _context.RegisterReceiver(receiver,new IntentFilter(BluetoothDevice.ActionFound));

            if (!_adapter.StartDiscovery())
            {
                _context.UnregisterReceiver(receiver);
                throw new Exception("无法发现设备");
            }

            var last_status = ConnectionStatus;
            ConnectionStatus = ConnectionStatusEnum.Discovering;



            var timeoutTask = Task.Delay(TimeSpan.FromSeconds(5));
            var completedTask = await Task.WhenAny(discoveryCompletion.Task, timeoutTask);

            if (completedTask == timeoutTask)
                _adapter.CancelDiscovery();
            
            _context.UnregisterReceiver(receiver);

            // 添加新发现的设备
            foreach (var device in receiver.DiscoveredDevices)
            {
                devices.Add(new(
                    name: device.Name ?? "Unknown",
                    address: device.Address ?? "Unknown",
                    isPaired: false
                ));
            }

            //旧设备
            if (_adapter.BondedDevices != null)
            {
                foreach (var d in _adapter.BondedDevices)
                {
                    devices.Add(new(
                        d.Name ?? "Unknown",
                        d.Address ?? "Unknown",
                        isPaired: true
                    ));
                }
            }

            ConnectionStatus = last_status;

            return devices;
        }

        public async Task SendDataAsync(Memory<byte> data)
        {
            if (ConnectionStatus != ConnectionStatusEnum.Connected)
                throw new InvalidOperationException("蓝牙未连接");

            ConnectionStatusEnum status = ConnectionStatus;
            ConnectionStatus = ConnectionStatusEnum.Sending;

            await _outputStream.WriteAsync(data);

            ConnectionStatus = status;
        }
        private async Task EnsureBluetoothReady()
        {
            if (_adapter == null)
                throw new NotSupportedException("设备不支持蓝牙");

            // 检查权限
            await Permissions.RequestAsync<Permissions.Bluetooth>();
            await Permissions.RequestAsync<Permissions.LocationWhenInUse>();    

            // 开启蓝牙
            if (!_adapter.IsEnabled)
            {
                var enableIntent = new Intent(BluetoothAdapter.ActionRequestEnable);
                enableIntent.SetFlags(ActivityFlags.NewTask);
                _context.StartActivity(enableIntent);

                await Task.Delay(3000);
                if (!_adapter.IsEnabled)
                    throw new Exception("蓝牙未开启");
            }
        }
        private async Task ListenForData(CancellationToken token)
        {
            var buffer = new byte[1024];

            try
            {
                while (!token.IsCancellationRequested)
                {
                    var bytesRead = await _inputStream.ReadAsync(buffer, 0, buffer.Length, token);
                    if (bytesRead > 0)
                    {
                        var data = new Memory<byte>(buffer, 0, bytesRead);
                        OnDataReceived?.Invoke(this, data);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // 正常退出
            }
            catch
            {
                await DisconnectAsync();
            }
        }

        private class BluetoothReceiver : BroadcastReceiver
        {
            public ConcurrentBag<BluetoothDevice> DiscoveredDevices { get; } = new();
            private readonly TaskCompletionSource<bool> _completionSource;

            public BluetoothReceiver(TaskCompletionSource<bool> completionSource)
            {
                _completionSource = completionSource;
            }

            public override void OnReceive(Context context, Intent intent)
            {
                if (intent.Action == BluetoothDevice.ActionFound)
                {
                    var device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                    if (device != null && !string.IsNullOrEmpty(device.Name))
                    {
                        DiscoveredDevices.Add(device);
                    }
                }
                else if (intent.Action == BluetoothAdapter.ActionDiscoveryFinished)
                {
                    _completionSource.TrySetResult(true);
                }
            }
        }
    }
}
