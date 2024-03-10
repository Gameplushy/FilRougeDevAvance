using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Interface
{
    public static class ChatClient
    {
        public static Socket ConnectToChat(string login)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Connect(new IPEndPoint(ip, 6969));

            s.Send(Encoding.Unicode.GetBytes($"{login}>Hello world"));
            return s;
        }
    }
}
