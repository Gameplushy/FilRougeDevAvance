using System.Windows;

namespace Interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            var res = ConnectionToLife.Connection.UserAuth.ConnectionToDatabse(tbUsername.Text, pbPassword.Password);
            if (!string.IsNullOrEmpty(res.error))
            {
                MessageBox.Show(res.error, "Connection error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                //MessageBox.Show($"Hello {res.res!.Username}!");
                var win = new Menu(res.res);
                Visibility = Visibility.Hidden;
                win.Show();
                this.Close();
            }
        }
    }
}