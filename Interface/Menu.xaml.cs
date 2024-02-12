using ConnectionToLife.Connection;
using ConnectionToLife.GameOfLife;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace Interface
{
    /// <summary>
    /// Logique d'interaction pour Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        private User user;
        private Thread iterationThread, chatThread;
        private Board b;
        private Socket s;

        public Menu(User user)
        {
            this.user = user;
            InitializeComponent();
            tbRules.Text = user.rules;
            b = new Board(user.rules);
            b.GenerateRandomBoard();
            s = Chat.ConnectToChat(user.user);
            chatThread = new Thread(new ThreadStart(() =>Listen(s)));
            chatThread.Start();
        }

        void Iterate()
        {
            try
            {

                this.Dispatcher.Invoke(() => tbGrid.Text = b.DisplayBoard());
                for (int i = 0; i < 10000; i++)
                {
                    GameRulesChecker.Iterate(b);
                    this.Dispatcher.Invoke(() => tbGrid.Text = b.DisplayBoard());
                    try
                    {
                        Thread.Sleep(500);
                    }
                    catch (ThreadInterruptedException tie) { break; }
                }
            }
            catch (Exception)
            {

            }
        }

        private void btnStartStop_Click(object sender, RoutedEventArgs e)
        {
            if (iterationThread == null)
            {
                iterationThread = new Thread(new ThreadStart(Iterate));
                iterationThread.Start();
            }
            else
            {
                iterationThread.Interrupt();
                iterationThread = null;
            }
        }

        private async void btnSend_Click(object sender, RoutedEventArgs e)
        {
            await s.SendAsync(System.Text.Encoding.Unicode.GetBytes($"{user.user}>{tbMessage.Text}"));
            tbMessage.Text = null;
        }
        public void Listen(Socket s)
        {
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[512];
                    s.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                    this.Dispatcher.Invoke(() => tbChat.Text += System.Text.UnicodeEncoding.Unicode.GetString(buffer)+'\n');
                    //Console.WriteLine(System.Text.UnicodeEncoding.Unicode.GetString(buffer));
                }
            }
            catch (SocketException se)
            {
                s.Close();
            }
        }
    }
}
