namespace HeraCrossController.Models
{
    public class BluetoothSerialDevice
    {
        string _name = "", _address = "";
        public string Name => _name;
        public string Address => _address;
        public static BluetoothSerialDevice Create(string name, string address)
        {
            return new()
            {
                _name = name,
                _address = address
            };
        }
    }
}
