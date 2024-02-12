﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            var res = ConnectionToLife.Connection.UserConnect.ConnectionToDatabse(tbUsername.Text, pbPassword.Password);
            if (!string.IsNullOrEmpty(res.error))
            {
                MessageBox.Show(res.error, "Connection error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show($"Hello {res.res!.user}!");
                var win = new Menu(res.res);
                Visibility = Visibility.Hidden;
                win.Show();
                this.Close();
            }
        }
    }
}