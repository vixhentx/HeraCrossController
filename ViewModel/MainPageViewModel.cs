using HeraCrossController.Model;
using HeraCrossController.Service;
using Microsoft.Maui.Controls.Compatibility.Platform.UWP;
using PropertyChanged;

namespace HeraCrossController.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class MainPageViewModel
    {
        public ConnectionStatusEnum ConnectionStatus => bluetoothService.ConnectionStatus;
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

        private readonly BluetoothService bluetoothService = new();
        public MainPageViewModel()
        {
            DataRecieved = string.Empty;
            DataToSend = string.Empty;

            _connectCommand = new Command(
                execute: () =>
                {
                    var devices = bluetoothService.ScanForDevices();
                    Console.WriteLine(devices.ToString());
                    RefreshCanExecutes();
                }
                );
            _sendDataCommand = new Command(
                execute: () =>
                {
                    throw new NotImplementedException();
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
                execute: (object arg) =>
                {
                    if (arg == null || arg is not int cmd) return;
                    
                },
                canExecute: (object arg) => ConnectionStatus == ConnectionStatusEnum.Connected
                );
        }

        void RefreshCanExecutes()
        {
            SendDataCommand.ChangeCanExecute();
            SendSimpleCommand.ChangeCanExecute();
        }
    }
}
