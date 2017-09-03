namespace MyFiles.Networking
{
    public static class NetworkManager
    {
        public static INetwork Network { private set; get; }

        public static void Initialize() {
            Network = new Net.Network();
        }
    }
}
