using MyFiles.Data;
using MyFiles.Networking;
using System;
using System.Threading;
using System.Windows.Threading;

namespace MyFiles
{
    public static class Program
    {
        // Main window
        public static AppWindow Window { private set; get; }

        public static readonly string StartupPath = AppDomain.CurrentDomain.BaseDirectory;

        public static void Main(string[] args) {
            NetworkManager.Initialize();
            DataManager.Load();

            var thread = new Thread(StartWindow);
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = false;
            thread.Start();
        }

        public static void Destroy() {
            DataManager.Save();
            Environment.Exit(1);
        }

        private static void StartWindow() {
            Window = new AppWindow();
            Dispatcher.Run();
        }

        public static void ShowMessage(string title, string message, bool shutdown = false) {
            Program.title = title;
            Program.message = message;
            Program.shutdown = shutdown;
            var thread = new Thread(ShowMessage);
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = false;
            thread.Start();
        }

        private static string title;
        private static string message;
        private static bool shutdown;
        private static void ShowMessage() {
            new AppMsgWindow(title, message).ShowDialog();
            if (shutdown) {
                Environment.Exit(1);
            }
        }
    }
}
