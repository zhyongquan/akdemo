using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace akserver
{
    class Program
    {
        static void Main(string[] args)
        {
            string ip = "127.0.0.1";
            int port = 7800;
            Console.WriteLine("input ip (default 127.0.0.1)");
            string str = Console.ReadLine();
            if (str.Length > 1)
                ip = str;
            Console.WriteLine("input port (default 7800)");
            str = Console.ReadLine();
            if (str.Length > 1)
                port = int.Parse(str);
            TcpListener server = new TcpListener(System.Net.IPAddress.Parse(ip), port);
            while (true)
            {
                server.Start();
                Console.WriteLine("start");
                TcpClient client = server.AcceptTcpClient();
                NetworkStream stream = client.GetStream();
                Console.WriteLine("client");
                byte[] buffer = new byte[1024];
                try
                {
                    while (true)
                    {
                        int len = stream.Read(buffer, 0, 1024);
                        if (len == 0) break;
                        byte[] bufferRecv = new byte[len];
                        Array.Copy(buffer, bufferRecv, len);
                        Console.WriteLine(Encoding.ASCII.GetString(bufferRecv));
                        //
                        Thread.Sleep(500);
                        stream.Write(bufferRecv, 0, bufferRecv.Length);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    server.Stop();
                    Console.WriteLine("stop");
                    Thread.Sleep(500);
                }
                Console.WriteLine("press esc key to exit, press other any to restart");
                ConsoleKeyInfo key= Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                    break;
            }
        }
    }
}
