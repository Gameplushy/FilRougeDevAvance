using ConnectionToLife.GameOfLife;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatServer
{
    public static class ChatServer
    {
        private static List<Socket> sockets = new();

        public static async Task ConnectToChat()
        {
            IPAddress ip = IPAddress.Parse("10.70.5.60");
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Bind(new IPEndPoint(ip, 6969));
            s.Listen(5);
            while (true)
            {
                Socket socketAccepted = await s.AcceptAsync();
                sockets.Add(socketAccepted);
                Thread listener = new Thread(() => Listen(socketAccepted));
                listener.Start();
            }
        }

        public static void Listen(Socket s)
        {
            try
            {
                s.Send(Encoding.Unicode.GetBytes("Connection established."));
                while (true)
                {
                    byte[] buffer = new byte[512];
                    s.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                    string res = Encoding.Unicode.GetString(buffer);
                    if (buffer[0] <= 1)
                    {
                        res = string.Join("", buffer.Take(Board.BOARDSIZE * Board.BOARDSIZE).Select(by => by == 1 ? 'X' : '·'));
                    }
                    sockets.ForEach(socket => socket.Send(Encoding.Unicode.GetBytes(res)));
                    Console.WriteLine(res);
                }
            }
            catch (SocketException se)
            {
                s.Close();
            }
        }
    }
}
