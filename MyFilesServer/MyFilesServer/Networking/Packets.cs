namespace MyFilesServer.Networking
{
    public enum Packet
    {
        // Add all new outgoing packet IDs before length.
        SendFinishedFile,
        Length
    }
}
