using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClassLibrary
{
    /// <summary>
    /// 
    /// </summary>
    public class ServerTCP : AbstractServer
    {
        public delegate void TransmissionDataDelegate(NetworkStream stream);
        public ServerTCP(string IPAddress = "127.0.0.1", int port = 8888, int bufferSize = 1024) : base(System.Net.IPAddress.Parse(IPAddress), port)
        {
            Buffer_size = bufferSize;
        }

        protected override void AcceptClient()
        {
            while (true)
            {
                TcpClient tcpClient = TcpListener.AcceptTcpClient();
                Stream = tcpClient.GetStream();
                TransmissionDataDelegate transmissionDelegate = new TransmissionDataDelegate(BeginDataTransmission);
                transmissionDelegate.BeginInvoke(Stream, TransmissionCallback, tcpClient);
            }
        }

        private void TransmissionCallback(IAsyncResult ar)
        {

        }

        protected override void BeginDataTransmission(NetworkStream networkStream)
        {
            Beginning(networkStream);

            while (true)
            {
                try
                {
                    StreamConverter converter = new StreamConverter();
                    string msg;
                    byte[] input = new byte[3];
                    networkStream.Read(input, 0, 3);
                    msg = Encoding.Default.GetString(input);
                    converter.checkStream(msg);
                    if (converter.check == "error")
                    {
                        Console.WriteLine("Can't read input!\n");
                        byte[] error = Encoding.Default.GetBytes("Can't read input!\n");
                        networkStream.Write(error, 0, error.Length);
                    }
                    else if (converter.check == "ok")
                    {
                        Console.WriteLine(Encoding.Default.GetString(input));
                        converter.Calculator(Encoding.Default.GetString(input));
                        string op = converter.answer.ToString() + "\n";
                        byte[] output = Encoding.Default.GetBytes(op);
                        networkStream.Write(output, 0, output.Length);
                        Console.WriteLine(Encoding.Default.GetString(output));
                    }
                }
                catch (IOException e)
                {
                    break;
                }
            }
        }

        public override void Start()
        {
            StartListening();
            AcceptClient();
        }

        public void Beginning(NetworkStream stream)
        {
            byte[] hello_msg = new byte[256];
            string hello_str = "Welcome!\r\n";
            hello_msg = new ASCIIEncoding().GetBytes(hello_str);
            stream.Write(hello_msg, 0, hello_str.Length);

            hello_str = "Type 3 characters, please use format XOX where X is digit (0 to 9) and X is the operation (+ or - or / or *)";
            hello_msg = new ASCIIEncoding().GetBytes(hello_str);
            stream.Write(hello_msg, 0, hello_str.Length);
        }
    }
}