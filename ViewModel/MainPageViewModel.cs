using HeraCrossController.Model;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HeraCrossController.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class MainPageViewModel
    {
        public ConnectionStatusEnum ConnectionStatus { get; set; }
        public string DataRecieved { get; set; }
        public string DataToSend { get; set; }

        Command _connectCommand, _sendDataCommand;

        [DoNotNotify]
        public Command ConnectCommand => _connectCommand;
        [DoNotNotify]
        public Command SendDataCommand => _sendDataCommand;

        public MainPageViewModel()
        {
            ConnectionStatus = ConnectionStatusEnum.Disconnected;
            DataRecieved ??= string.Empty;
            DataToSend ??= string.Empty;

            _connectCommand = new Command(
                execute: () =>
                {
                    RefreshCanExecutes();
                    throw new NotImplementedException();
                }
                );
            _sendDataCommand = new Command(
                execute: () =>
                {
                    throw new NotImplementedException();
                },
                canExecute: () => ConnectionStatus== ConnectionStatusEnum.Connected
                );
        }

        void RefreshCanExecutes()
        {
            ConnectCommand.ChangeCanExecute();
        }
    }
}
