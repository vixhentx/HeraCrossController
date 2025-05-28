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

namespace HeraCrossController.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class MainPageViewModel
    {
        public ConnectionStatusEnum ConnectionStatus { get; set; }
        public string DataRecieved { get; set; }
        public string DataToSend { get; set; }



        public MainPageViewModel()
        {
            ConnectionStatus = ConnectionStatusEnum.Disconnected;
            DataRecieved ??= string.Empty;
            DataToSend ??= string.Empty;
        }
    }
}
