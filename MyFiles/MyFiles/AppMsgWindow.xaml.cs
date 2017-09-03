using System.Windows;

namespace MyFiles
{
    public partial class AppMsgWindow : Window
    {
        public AppMsgWindow(string title, string message) {
            InitializeComponent();

            this.Title = title;
            lblMessage.Content = message;
            cmdOkay.Click += CmdOkay_Click;
        }

        private void CmdOkay_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
