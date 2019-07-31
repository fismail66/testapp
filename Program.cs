using System;
using System.Net.Sockets;
using System.Text;

namespace testapp
{
    class Program
    {
        static void Main(string[] args)
        {
            string server = "127.0.0.1";
            int port = 906;
            if (args.Length == 2)
            {
                server = args[0];
                port = Convert.ToInt32(args[1]);
            }

            TcpClient tcpClient = new TcpClient();
            try
            {
                tcpClient.Connect(server, port);
                GetDataFromScale(tcpClient);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Read();
            }
        }

        private static void GetDataFromScale(TcpClient tcpClient)
        {
            // Get the stream used to read the message sent by the server.
            NetworkStream networkStream = tcpClient.GetStream();
            // Set a 10 millisecond timeout for reading.
            networkStream.ReadTimeout = 10;
            networkStream.WriteTimeout = 10;
            byte[] msg = new byte[] { 0x42, 0x13, 0x10 };
            int readBytes = 0;

            // Read the server message into a byte buffer.
            byte[] bytes = new byte[1024];
            using (tcpClient)
            using (networkStream)
            {
                for (; ; )
                {
                    try
                    {
                        networkStream.Write(msg, 0, msg.Length);
                        readBytes = networkStream.Read(bytes, 0, 1024);
                        //Convert the server's message into a string and display it.
                        string data = Encoding.UTF8.GetString(bytes, 0, readBytes);
                        Console.WriteLine("Server sent message: {0}", data);
                    }
                    catch { }

                }
            }
        }
    }
}
