﻿using HeraCrossController.Interfaces;
using HeraCrossController.Models;
using PropertyChanged;
using System.Text;

namespace HeraCrossController.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MainPageViewModel
    {
        public ConnectionStatusEnum ConnectionStatus { get; set; }
        public string DataRecieved { get; set; }
        public string DataToSend { get; set; }

        protected readonly Command _connectCommand, _sendDataCommand, _clearCommand, _sendSimpleCommand;

        [DoNotNotify]
        public Command ConnectCommand => _connectCommand;
        [DoNotNotify]
        public Command SendDataCommand => _sendDataCommand;
        [DoNotNotify]
        public Command ClearCommand => _clearCommand;
        [DoNotNotify]
        public Command SendSimpleCommand => _sendSimpleCommand;


        private readonly IBluetoothSerial _serial;
        private readonly IDialogService _dialogService;
        public MainPageViewModel(IBluetoothSerial bluetoothSerial,IDialogService dialogService)
        {
            _serial = bluetoothSerial;
            _dialogService = dialogService;
            DataRecieved = string.Empty;
            DataToSend = string.Empty;

            _serial.OnDataReceived += OnRecievedData;
            _serial.OnConnectionStatusChanged += (sender, e) =>
            {
                ConnectionStatus = e;
                RefreshCanExecutes();
            };

            _connectCommand = new Command(
                execute: async () =>
                {
                    DataRecieved += "发现设备中:\n";
                    try
                    {
                        var all_devices = (await _serial.DiscoverDevicesAsync());
                        List<BluetoothSerialDevice>? devices = all_devices?.Where((d) => d.Name == "HC-02").ToList();
                        DataRecieved += "发现的设备:\n";
                        if (devices == null || devices.Count == 0)
                        {
                            dialogService.ShowMessageBox("找不到设备");
                            return;
                        }
                        foreach (var device in devices)
                        {
                            DataRecieved += $"Name: {device.Name}, Address: {device.Address} \n";
                        }
                        var target = await dialogService.ShowDevicesAsync(devices);
                        if (target == null)
                        {
                            DataRecieved += "未选择设备 \n";
                            return;
                        }
                        await _serial.ConnectAsync(target);

                    }
                    catch(Exception ex)
                    {
                        string str = $"异常: { ex.Message} \n";
                        DataRecieved += str;
                        _dialogService.ShowMessageBox(str,"异常");
                    }
                }
                );
            _sendDataCommand = new Command(
                execute: async () =>
                {
                    byte[] data = Encoding.UTF8.GetBytes(DataToSend + "\n");
                    await _serial.SendDataAsync(new Memory<byte>(data));
                    DataToSend = "";
                },
                canExecute: () => ConnectionStatus == ConnectionStatusEnum.Connected
                );
            _clearCommand = new Command(
                execute: () =>
                {
                    DataRecieved = "";
                }
                );
            _sendSimpleCommand = new Command(
                execute: async (object arg) =>
                {
                    if (arg == null || arg is not int cmd) return;
                    byte[] data = Encoding.UTF8.GetBytes(cmd.ToString() + "\n");
                    await _serial.SendDataAsync(new Memory<byte>(data));
                },
                canExecute: (object arg) => ConnectionStatus == ConnectionStatusEnum.Connected
                );
        }

        void RefreshCanExecutes()
        {
            SendDataCommand.ChangeCanExecute();
            SendSimpleCommand.ChangeCanExecute();
        }
        void OnRecievedData(object? sender, Memory<byte> e)
        {
            DataRecieved+=$"[HC-02]: {Encoding.UTF8.GetString(e.ToArray(), 0, e.Length)} \n";
        }
    }
}
