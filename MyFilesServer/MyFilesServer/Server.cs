using System;

namespace MyFilesServer
{
    public static class Server
    {
        public static readonly string StartupPath = AppDomain.CurrentDomain.BaseDirectory;

        private static void Main(string[] args) {

        }

        public static void Write(string input) {
            Console.WriteLine(input);
        }
    }
}
