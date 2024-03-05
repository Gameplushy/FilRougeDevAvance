using System.Net.Http.Headers;
using System.Net.Http;
using System.Windows;
using ConnectionToLife.Connection;
using System.Net.Http.Json;
using APICTL.Models;

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

        private async void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            //var res;// = APICTL.Controllers.AuthenticationController//ConnectionToLife.Connection.UserAuth.ConnectionToDatabse(tbUsername.Text, pbPassword.Password);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5085/");
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PostAsJsonAsync("/Authentication",new Credentials() { Login = tbUsername.Text, Password = pbPassword.Password}); //API controller name
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<User>();
                    //MessageBox.Show($"Hello {res.res!.Username}!");
                    var win = new Menu(result);
                    Visibility = Visibility.Hidden;
                    win.Show();
                    this.Close();
                    
                }
                else
                {
                    MessageBox.Show("Bad credentials", "Connection error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
    }
}