using HeraCrossController.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeraCrossController.ViewModel
{
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
