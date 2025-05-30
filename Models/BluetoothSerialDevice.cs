using HeraCrossController.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeraCrossController.Models
{
    public class BluetoothSerialDevice
    {
        string _name = "", _uuid = "";
        bool _paired = false;
        public string Name => _name;
        public string UUID => _uuid;
        public bool IsPaired => _paired;
    }
}
