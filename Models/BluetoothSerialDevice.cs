namespace HeraCrossController.Models
{
    public class BluetoothSerialDevice
    {
        string _name = "", _address = "";
        bool _isPaired = false ;
        public string Name => _name;
        public string Address => _address;
        public bool IsPaired => _isPaired;

        public BluetoothSerialDevice(string name, string address, bool isPaired = false)
        {
            _name = name;
            _address = address;
            _isPaired = isPaired;
        }

    }
}
