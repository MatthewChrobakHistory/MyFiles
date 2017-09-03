namespace MyFiles.Networking
{
    public interface INetwork
    {
        void Destroy();
        void SendData(byte[] array);
    }
}
