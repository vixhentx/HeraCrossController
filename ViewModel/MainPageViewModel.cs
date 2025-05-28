using HeraCrossController.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeraCrossController.ViewModel
{
    public class MainPageViewModel
    {
        ConnectionStatusEnum _connectionStatus= ConnectionStatusEnum.Disconnected;
        public string ConnectionStatus => _connectionStatus.ToString();
        public string DataRecieved { get; set; }
        public string DataToSend { get; set; }



        public MainPageViewModel()
        {
            DataRecieved ??= string.Empty;
            DataToSend ??= string.Empty;
        }
    }
}
