using MyFiles.Data;
using System.Windows;

namespace MyFiles
{
    public partial class AppWindow : Window
    {
        // Tray icon
        private System.Windows.Forms.NotifyIcon trayIcon = null;

        public AppWindow() {
            InitializeComponent();

            // Form events
            this.IsVisibleChanged += AppWindow_IsVisibleChanged;
            this.Closing += AppWindow_Closing;

            // Element events.
            this.cmdSave.Click += CmdSave_Click;
            this.cmdSaveBasePath.Click += CmdSaveBasePath_Click;

            // Set up the tray system.
            this.SetupTraySystem();
        }

        private void AppWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) {
            if ((bool)e.NewValue) {
                this.txtBasePath.Text = DataManager.Settings.MyFilesPath; 
                this.txtUsername.Text = DataManager.Settings.Username;
                this.txtPassword.Password = DataManager.Settings.Password;
            }
        }

        private void CmdSaveBasePath_Click(object sender, RoutedEventArgs e) {
            DataManager.ChangeBasePath(this.txtBasePath.Text);
        }

        private void CmdSave_Click(object sender, RoutedEventArgs e) {
            DataManager.SetDetails(txtUsername.Text, txtPassword.Password);
        }

        private void AppWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
            this.Hide();
        }

        private void SetupTraySystem() {
            this.trayIcon = new System.Windows.Forms.NotifyIcon();
            this.trayIcon.Icon = new System.Drawing.Icon(System.AppDomain.CurrentDomain.BaseDirectory + "icon.ico");

            this.trayIcon.ContextMenu = new System.Windows.Forms.ContextMenu();
            this.trayIcon.ContextMenu.MenuItems.Add("Show Application");
            this.trayIcon.ContextMenu.MenuItems.Add("Quit");
            this.trayIcon.ContextMenu.MenuItems[0].Click += AppWindow_ShowApplication;
            this.trayIcon.ContextMenu.MenuItems[1].Click += AppWindow_Quit;

            this.trayIcon.Visible = true;
        }

        private void AppWindow_Quit(object sender, System.EventArgs e) {
            Program.Destroy();
        }

        private void AppWindow_ShowApplication(object sender, System.EventArgs e) {
            Program.Window.Show();
        }
    }
}
