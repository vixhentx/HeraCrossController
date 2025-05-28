using Android.Bluetooth;
using HeraCrossController.Interface;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HeraCrossController.Interface.IBluetoothService;

namespace HeraCrossController.Platforms.Android
{
    public class BluetoothService : IBluetoothService
    {
        private BluetoothSocket? _socket;
        private ReciveDataDelegate OnDataRecieved;
        public BluetoothService(ReciveDataDelegate handler)
        {
            OnDataRecieved = handler;
        }
        public void SetRecieveDataHandler(ReciveDataDelegate handler)
        {
            OnDataRecieved = handler;
        }

        public async Task<bool> Connect(string deviceName)
        {
            try
            {
                // 获取蓝牙适配器
                var adapter = BluetoothAdapter.DefaultAdapter;
                if (adapter == null) return false;

                // 查找配对设备
                var device = adapter.BondedDevices?
                    .FirstOrDefault(d => d.Name == deviceName);

                if (device == null) return false;

                // 创建 SPP 连接 (标准UUID)
                _socket = device.CreateRfcommSocketToServiceRecord(
                    UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));

                if (_socket == null) return false;

                // 建立连接
                await _socket.ConnectAsync();

                // 启动数据监听线程
                new Thread(Receive).Start();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public void Receive()
        {
            var buffer = new byte[256];
            while (_socket?.IsConnected == true)
            {
                try
                {
                    var bytesRead = _socket.InputStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        List<byte> ret = [];
                        for (int i = 0; i < bytesRead; i++)
                        {
                            ret.Add(buffer[i]);
                        }
                        MainThread.BeginInvokeOnMainThread(() => OnDataRecieved(ret));
                    }
                }
                catch(Exception ex) 
                {
                    Console.WriteLine(ex.ToString());
                    break;
                }
            }
        }

        public async Task Send(List<byte> message)
        {
            if (_socket?.IsConnected != true) return;

            byte[]bytes= message.ToArray();
            await _socket.OutputStream.WriteAsync(bytes, 0, bytes.Length);
        }
    }
}
