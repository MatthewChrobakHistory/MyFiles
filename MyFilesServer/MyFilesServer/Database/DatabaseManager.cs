namespace MyFilesServer.Database
{
    public static class DatabaseManager
    {
        public static IDatabase Database { private set; get; }

        public static void Initialize() {
            Database = new MySQL.MySQL();
        }

        public static void Destroy() {
            Database.Close();
        }
    }
}
