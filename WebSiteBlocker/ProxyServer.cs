using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WebSiteBlocker.Classes;
using WebSiteBlocker.Itrefaces;

namespace WebSiteBlocker
{
    class ProxyServer:IOutputObserver
    {
            Socket clientSocket;
            Output output;
            string _message;
            Byte[] read = new byte[1024];
            Byte[] Buffer = null;
            private int port = 8889;
            Encoding ASCII = Encoding.ASCII;
            const string HTTP_VERSION = "HTTP/1.0";
            const string CRLF = "\r\n";
            Byte[] RecvBytes = new Byte[4096];

        public ProxyServer(Socket s,Output o)
        {
            clientSocket = s;
            output = o;
        }

            public ProxyServer()
            {
            }

            public void runProxy()
            {
                String clientmessage = " ", sURL = " ";
                int bytes = readmessage(read, ref clientSocket, ref clientmessage);

                if (bytes == 0)
                {
                    return;
                }

                int index1 = -1;
                int index2 = -1;
                try
                {
                    index1 = clientmessage.IndexOf(' ');
                    index2 = clientmessage.IndexOf(' ', index1 + 1);
                    if ((index1 == -1) || (index2 == -1))
                    {
                        throw new System.IO.IOException();
                    }
                }
                catch (Exception e)
                {
                    e.ToString();
                }
                //Console.WriteLine("Connecting to Site: {0}", clientmessage);
                _message = "Connecting to Site: " + clientmessage;
                output.publishMessage(_message);
                
                //Console.WriteLine("Connecting to Site: {0}", clientmessage.Substring(index1 + 1, index2 - index1));
                Console.WriteLine("Connection from {0}", clientSocket.RemoteEndPoint);
                _message = "Connection from {0}"+ clientSocket.RemoteEndPoint;
                output.publishMessage(_message);


            try
                {
                    string part1 = clientmessage.Substring(index1 + 1, index2 - index1);
                    int index3 = part1.IndexOf('/', index1 + 8);
                    int index4 = part1.IndexOf(' ', index1 + 8);
                    int index5 = index4 - index3;
                    sURL = part1.Substring(index1 + 4, (part1.Length - index5) - 8);
                    IPHostEntry IPHost = Dns.GetHostEntry(sURL);
                    Console.WriteLine("Request resolved: ", IPHost.HostName);
                    string[] aliases = IPHost.Aliases;
                    IPAddress[] address = IPHost.AddressList;
                    Console.WriteLine(address[0]);
                    IPEndPoint sEndpoint = new IPEndPoint(address[0], 80);
                    Socket IPsocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPsocket.Connect(sEndpoint);
                if (IPsocket.Connected)
                {
                    Console.WriteLine("Socket connect OK");
                }
                    
                    string GET = clientmessage;
                    Byte[] ByteGet = ASCII.GetBytes(GET);
                    IPsocket.Send(ByteGet, ByteGet.Length, 0);
                    Int32 rBytes = IPsocket.Receive(RecvBytes, RecvBytes.Length, 0);
                    Console.WriteLine("Recieved {0}", +rBytes);
                    Buffer = RecvBytes;
                    String strRetPage = null;
                    strRetPage = strRetPage + ASCII.GetString(RecvBytes, 0, rBytes);
                    while (rBytes > 0)
                    {
                        rBytes = IPsocket.Receive(RecvBytes, RecvBytes.Length, 0);
                        strRetPage = strRetPage + ASCII.GetString(RecvBytes, 0, rBytes);
                    }
                    IPsocket.Shutdown(SocketShutdown.Both);
                    IPsocket.Close();
                    sendmessage(clientSocket, strRetPage);
                }
                catch (Exception exc2)
                {
                    Console.WriteLine(exc2.ToString());
                }
            }

            private int readmessage(byte[] ByteArray, ref Socket s, ref String clientmessage)
            {
                int bytes = s.Receive(ByteArray, 1024, 0);
                string messagefromclient = Encoding.ASCII.GetString(ByteArray);
                clientmessage = (String)messagefromclient;
                return bytes;
            }
            private void sendmessage(Socket s, string message)
            {
                Buffer = new Byte[message.Length + 1];
                int length = ASCII.GetBytes(message, 0, message.Length, Buffer, 0);
                s.Send(Buffer, length, 0);
            }

            public void run()   //simple method to listen on ip and port
        {
            try
            {
                byte[] input = BitConverter.GetBytes(1);
                byte[] buffer = new byte[4096];
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
                clientSocket.Bind(new IPEndPoint(IPAddress.Parse("192.168.1.213"), 8889));
                clientSocket.IOControl(IOControlCode.ReceiveAll, input, null);

                int bytess = 0;
                do
                {
                    bytess = clientSocket.Receive(buffer);
                    if (bytess > 0)
                    {
                        Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, bytess));
                        clientSocket.Send(buffer,bytess,0);
                    }
                } while (bytess > 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void Update()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)delegate ()
            {
                var context = Application.Current.MainWindow as MainWindow;
                context.ConsoleTextBlock.Text += _message + "\n";
            });
        }
    }
    
}
