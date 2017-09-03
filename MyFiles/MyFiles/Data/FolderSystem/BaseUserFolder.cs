using System.IO;

namespace MyFiles.Data.FolderSystem
{
    public class BaseUserFolder
    {
        private FileSystemWatcher _watcher;

        public BaseUserFolder(string path) {
            _watcher = new FileSystemWatcher(path);
            _watcher.EnableRaisingEvents = true;

            _watcher.NotifyFilter = NotifyFilters.Attributes |
                NotifyFilters.CreationTime |
                NotifyFilters.FileName |
                NotifyFilters.LastAccess |
                NotifyFilters.LastWrite |
                NotifyFilters.Size |
                NotifyFilters.Security;

            _watcher.Created += new FileSystemEventHandler(watcher_Created);
            _watcher.Changed += new FileSystemEventHandler(watcher_Changed);
            _watcher.Deleted += new FileSystemEventHandler(watcher_Deleted);
            _watcher.Renamed += _watcher_Renamed;
        }

        private void _watcher_Renamed(object sender, RenamedEventArgs e) {
            System.Console.WriteLine(e.ChangeType + ": From " + e.OldName + " to " + e.Name);
        }

        private void watcher_Deleted(object sender, FileSystemEventArgs e) {
            System.Console.WriteLine(e.ChangeType + ": " + e.Name);
        }

        private void watcher_Changed(object sender, FileSystemEventArgs e) {
            System.Console.WriteLine(e.ChangeType + ": " + e.Name);
        }

        private void watcher_Created(object sender, FileSystemEventArgs e) {
            System.Console.WriteLine(e.ChangeType + ": " + e.Name);
        }
    }
}
