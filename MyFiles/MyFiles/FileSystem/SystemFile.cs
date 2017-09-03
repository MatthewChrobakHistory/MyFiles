using System;
using System.IO;

namespace MyFiles.FileSystem
{
    public class SystemFile
    {
        public string FileName { private set; get; }
        public string Extension { private set; get; }
        public long Size { private set; get; }
        public DateTime LastWrite { private set; get; }

        private SystemFile() {

        }

        public SystemFile(string fileName, string extension, long size, DateTime lastWrite) {
            this.FileName = fileName;
            this.Extension = extension;
            this.Size = size;
            this.LastWrite = lastWrite;
        }

        public static bool Create(string path, out SystemFile file) {
            file = null;

            // Make sure it exists.
            if (!File.Exists(path)) {
                return false;
            }

            var fi = new FileInfo(path);
            fi.last
        }

        public SystemFile(string path) {
            var fi = new FileInfo(path);

        }
    }
}
