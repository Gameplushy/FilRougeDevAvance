using APICTL.Models;
using System.Net.Http;
using System.Net.Http.Json;
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

        private bool isIterating = false;

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

        public async void Iterate()
        {
            try
            {
                this.Dispatcher.Invoke(() => MakeGrid(board.ToString()));
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5085/");
                    while (true)
                    {
                        HttpResponseMessage response = await client.PostAsJsonAsync("/Board", new BoardRequest(board));
                        if (!isIterating) break;
                        if (response.IsSuccessStatusCode)
                        {
                            board.FromString(await response.Content.ReadAsStringAsync());
                            this.Dispatcher.Invoke(() => MakeGrid(board.ToString()));
                        }

                        try
                        {
                            Thread.Sleep(500);
                        }
                        catch (ThreadInterruptedException tie) { break; }
                    }
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
                isIterating = true;
                iterationThread.Start();
            }
            else
            {
                iterationThread.Interrupt();
                isIterating = false;
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
            int i = 0;
            foreach(Rectangle rec in gridUI)
            {
                Dispatcher.Invoke(() => rec.Fill = gridText[i] == 'X' ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.White));
                i++;
            }
            board.FromString(gridText.Substring(0, Board.BOARDSIZE * Board.BOARDSIZE));

        }
    }
}
