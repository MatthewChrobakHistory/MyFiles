namespace MyFilesServer.Database
{
    public interface IDatabase
    {
        bool Query(string query);
        bool NonQuery(string command);
        object GetValue(string key);
        bool NextEntry();
        void Close();
    }
}
