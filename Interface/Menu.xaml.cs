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
        private Thread? iterationThread, chatThread;
        private Board board;
        private Socket socket;

        public Menu(User user)
        {
            this.user = user;
            InitializeComponent();
            tbRules.Text = user.Rules;
            board = new Board(user.Rules);
            board.GenerateRandomBoard();
            socket = ChatClient.ConnectToChat(user.Username);
            chatThread = new Thread(new ThreadStart(() =>Listen(socket)));
            chatThread.Start();
        }

        void Iterate()
        {
            try
            {

                this.Dispatcher.Invoke(() => tbGrid.Text = board.DisplayBoard());
                for (int i = 0; i < 10000; i++)
                {
                    GameRulesChecker.Iterate(board);
                    this.Dispatcher.Invoke(() => tbGrid.Text = board.DisplayBoard());
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

        private async void btnStartStop_Click(object sender, RoutedEventArgs e)
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
                await socket.SendAsync(board.ToBytes());
            }
        }

        private async void btnSend_Click(object sender, RoutedEventArgs e)
        {
            await socket.SendAsync(Encoding.Unicode.GetBytes($"{user.Username}>{tbMessage.Text}"));
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
                    iterationThread?.Interrupt();
                    string message = Encoding.Unicode.GetString(buffer);
                    if (message.StartsWith("NEWRULE>"))
                    {
                        string newRule = message.Split(">")[1];
                        board.SetRules(newRule);
                        Dispatcher.Invoke(() => tbRules.Text = newRule);
                    }
                    else if (message.Take(Board.BOARDSIZE*Board.BOARDSIZE).All(c=>c=='X'||c== '·'))
                    {
                        StringBuilder sb = new();
                        for(int i =0; i<(Board.BOARDSIZE*Board.BOARDSIZE); i += Board.BOARDSIZE)
                        {
                            sb.AppendLine(message.Substring(i, Board.BOARDSIZE));
                        }
                        Dispatcher.Invoke(() => tbGrid.Text = sb.ToString());
                        board.FromString(message.Substring(0, Board.BOARDSIZE * Board.BOARDSIZE));
                    }
                    else
                    {
                        Dispatcher.Invoke(() => tbChat.Text += message + '\n');
                    }
                }
            }
            catch (SocketException se)
            {
                s.Close();
            }
        }
    }
}
