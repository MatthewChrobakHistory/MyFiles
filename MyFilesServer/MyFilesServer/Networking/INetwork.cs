namespace MyFilesServer.Networking
{
    public interface INetwork
    {
        void Destroy();
        void SendDataTo(int index, byte[] array);
        void SendDataToAll(byte[] array);
    }
}
