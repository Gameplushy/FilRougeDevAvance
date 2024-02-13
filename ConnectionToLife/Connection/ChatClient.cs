﻿using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConnectionToLife.Connection
{
    public static class ChatClient
    {
        public static async Task<Socket> ConnectToChatAsync(string login)
        {
            IPAddress ip = IPAddress.Parse("10.70.5.60");
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await s.ConnectAsync(new IPEndPoint(ip, 6969));

            await s.SendAsync(Encoding.Unicode.GetBytes($"{login}>Hello world"));
            return s;
        }

        public static Socket ConnectToChat(string login)
        {
            IPAddress ip = IPAddress.Parse("10.70.5.60");
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Connect(new IPEndPoint(ip, 6969));

            s.Send(Encoding.Unicode.GetBytes($"{login}>Hello world"));
            return s;
            //s.Close();def
            //Socket s = new Socket();
        }

        internal static bool Talk(Socket socketToTalk, string user)
        {
            Console.Write("Ecrire un message :");
            string? message = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(message))
                return false;
            socketToTalk.Send(Encoding.Unicode.GetBytes($"{user}>{message}"));
            return true;
        }
    }
}