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

        protected override void BeginDataTransmission(NetworkStream stream)
        {
            Beginning(stream);
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