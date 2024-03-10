using APICTL.Models;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace ChatServer
{
    public static class ChatServer
    {
        private static List<Socket> sockets = new();

        //Utilisé pour le vote d'une nouvelle règle
        private static (string newRule, bool[] votes)? vote = null;

        public static async Task ConnectToChat()
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Bind(new IPEndPoint(ip, 6969));
            s.Listen(5);
            while (true)
            {
                try
                {
                    Socket socketAccepted = await s.AcceptAsync();
                    sockets.Add(socketAccepted);
                    Thread listener = new Thread(() => Listen(socketAccepted));
                    vote = null;
                    listener.Start();
                }
                catch (Exception)
                {
                    continue;
                }
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
                    string res = string.Join("", Encoding.Unicode.GetString(buffer).TakeWhile(c => c != 0));
                    //Nouvelle grille
                    if (res == null || buffer[0] <= 1)
                    {
                        res = string.Join("", buffer.Take(Board.BOARDSIZE * Board.BOARDSIZE).Select(by => by == 1 ? 'X' : '·'));
                    }
                    //Proposition de nouvelle règle
                    else if (vote==null && Regex.IsMatch(res, @"^.*\>RULE=[0-9]+A[0-9]+D$"))
                    {
                        bool[] bl = new bool[sockets.Count];
                        bl[sockets.IndexOf(s)] = true;
                        vote = (res.Split("=")[1], bl);
                    }
                    //Vote
                    else if(vote != null && Regex.IsMatch(res, @"^.*\>[yn]$"))
                    {
                        //Refus = on annule le vote
                        if (res.Last() == 'n')
                            vote = null;
                        //Accepté = Si tous valident = envoi de la nouvelle règle
                        else
                        {
                            vote.Value.votes[sockets.IndexOf(s)] = true;
                            if (vote.Value.votes.All(b => b))
                            {
                                sockets.ForEach(socket => socket.Send(Encoding.Unicode.GetBytes($"NEWRULE>{vote.Value.newRule}")));
                                vote = null;
                            }
                        }
                    }
                    sockets.ForEach(socket => socket.Send(Encoding.Unicode.GetBytes(res)));
                    Console.WriteLine(res);
                }
            }
            catch (SocketException)
            {
                s.Close();
            }
        }
    }
}
