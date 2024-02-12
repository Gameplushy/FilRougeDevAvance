using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public static class Chat
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
                Thread listener = new Thread(()=>Listen(socketAccepted));
                listener.Start();
            }
        }

        public static void Listen(Socket s)
        {
            try
            {
                s.Send(System.Text.Encoding.Unicode.GetBytes("Connection established."));
                while (true)
                {
                    byte[] buffer = new byte[512];
                    s.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                    sockets.ForEach(socket => socket.Send(buffer));
                    Console.WriteLine(System.Text.UnicodeEncoding.Unicode.GetString(buffer));
                }
            }
            catch(SocketException se)
            {
                s.Close();
            }
        }
    }
}
