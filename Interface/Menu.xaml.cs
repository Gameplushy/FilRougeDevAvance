using ConnectionToLife.Connection;
using ConnectionToLife.GameOfLife;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

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
        private Rectangle[,] gridUI = new Rectangle[Board.BOARDSIZE,Board.BOARDSIZE];

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
            for(int i = 0; i < Board.BOARDSIZE; i++)
            {
                gdGOL.ColumnDefinitions.Add(new ColumnDefinition());
                gdGOL.RowDefinitions.Add(new RowDefinition());
            }
            for(int i = 0; i < Board.BOARDSIZE; i++)
            {
                for(int j=0;j < Board.BOARDSIZE; j++)
                {
                    Rectangle r = new Rectangle()
                    {
                        VerticalAlignment = VerticalAlignment.Stretch,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Fill = new SolidColorBrush(Colors.White),
                        Stroke = new SolidColorBrush(Colors.Black)
                    };
                    Grid.SetRow(r, i);
                    Grid.SetColumn(r, j);
                    gridUI[i, j] = r;
                    gdGOL.Children.Add(r);
                }
            }
        }

        void Iterate()
        {
            try
            {

                this.Dispatcher.Invoke(() => MakeGrid(board.DisplayBoard()));
                for (int i = 0; i < 10000; i++)
                {
                    GameRulesChecker.Iterate(board);
                    this.Dispatcher.Invoke(() => MakeGrid(board.DisplayBoard()));
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
                        MakeGrid(message);
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

        private void MakeGrid(string gridText)
        {
            for(int n = 0; n < Board.BOARDSIZE * Board.BOARDSIZE; n++)
            {
                Dispatcher.Invoke(() => gridUI[n / Board.BOARDSIZE, n % Board.BOARDSIZE].Fill = gridText[n] == 'X' ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.White));
                
            }
            /*StringBuilder sb = new();
            for (int i = 0; i < (Board.BOARDSIZE * Board.BOARDSIZE); i += Board.BOARDSIZE)
            {
                sb.AppendLine(gridText.Substring(i, Board.BOARDSIZE));
            }
            Dispatcher.Invoke(() => tbGrid.Text = sb.ToString());*/
            board.FromString(gridText.Substring(0, Board.BOARDSIZE * Board.BOARDSIZE));

        }
    }
}
