namespace HeraCrossController.Interface
{
    public interface IBluetoothService
    {
        delegate void ReciveDataDelegate(List<byte> data);
        public Task<bool> Connect(string deviceName);
        public Task Send(List<byte> message);
        public void Receive();
        public void SetRecieveDataHandler(ReciveDataDelegate handler);

    }
}