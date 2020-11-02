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
            using (Database db = new Database())
            {
                Login(networkStream, db);

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
                            Console.WriteLine("Can't read input!\r\n");
                            byte[] error = Encoding.Default.GetBytes("Can't read input!\r\n");
                            networkStream.Write(error, 0, error.Length);
                        }
                        else if (converter.check == "ok")
                        {
                            Console.WriteLine(Encoding.Default.GetString(input));
                            converter.Calculator(Encoding.Default.GetString(input));
                            string op = converter.answer.ToString() + "\r\n";
                            byte[] output = Encoding.Default.GetBytes(op);
                            networkStream.Write(output, 0, output.Length);
                            Console.WriteLine(Encoding.Default.GetString(output));
                        }
                        else if(converter.check == "db")
                        {
                            string dbmsg = db.ReadData();
                            dbmsg += "\r\n";    
                            byte[] output = Encoding.Default.GetBytes(dbmsg);
                            networkStream.Write(output, 0, output.Length);
                        }
                    }
                    catch (IOException e)
                    {
                        break;
                    }
                }
            }
          
        }

        public override void Start()
        {
            StartListening();
            AcceptClient();
        }


        public void Login(NetworkStream stream, Database db)
        {
            string name;
            byte[] msg = new byte[256];
            byte[] msg1 = new byte[256];
            string str = "Welcome! Type your name:\r\n";
            msg = new ASCIIEncoding().GetBytes(str);
            stream.Write(msg, 0, str.Length);
        
            stream.Read(msg1, 0, msg1.Length);
            name = Encoding.Default.GetString(msg1);

            db.InsertData(name);
            str = "Type 3 characters, use format XOX where X is digit (0 to 9) and X is the operation (+ or - or / or *)\r\n" +
                "To see your name, type \"msg\"\r\n";

            msg = new ASCIIEncoding().GetBytes(str);
            stream.Write(msg, 0, str.Length);
        }
    }
}