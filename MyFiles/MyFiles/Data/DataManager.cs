using MyFiles.Data.FolderSystem;
using System.IO;

namespace MyFiles.Data
{
    public static class DataManager
    {
        public static Settings Settings { private set; get; }
        public static BaseUserFolder Folder { private set; get; }

        public static void Load() {
            Settings = Settings.Load();

            if (Directory.Exists(Settings.MyFilesPath)) {
                Folder = new BaseUserFolder(Settings.MyFilesPath);
            }
        }

        public static void Save() {
            Settings.Save(Settings);
        }

        public static void SetDetails(string username, string password) {
            Settings.Username = username;
            Settings.Password = password;
            Program.ShowMessage("Action", "Details successfully set.");
        }

        public static void ChangeBasePath(string path) {
            if (Directory.Exists(path)) {
                Settings.MyFilesPath = path;
                Folder = new BaseUserFolder(path);
                Program.ShowMessage("Action", "Path set to " + path);
            } else {
                Program.ShowMessage("Error", "Path does not exist!");
            }
        }
    }
}
