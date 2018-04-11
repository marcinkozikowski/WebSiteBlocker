using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WebSiteBlocker.Classes;
using WebSiteBlocker.Itrefaces;
using static WebSiteBlocker.MainWindow;

namespace WebSiteBlocker
{
    class TcpListenerThread : IOutputObserver
    {
        int port = 8889;
        bool isActive = false;
        TcpListener tcplistener;
        string _message;
        Output output;

        public TcpListenerThread(int portN,Output o)
        {
            port = portN;
            isActive = true;
            output = o;
        }
        public TcpListenerThread()
        {

        }
        public void run()
        {
            
            tcplistener = new TcpListener(port);
            //Console.WriteLine("Listening on port {0}", +port);
            _message = "Listening on port " +port;
            output.publishMessage(_message);
            tcplistener.Start();
            while (isActive)
            {
                try
                {
                    Socket socket = tcplistener.AcceptSocket();
                    ProxyServer webproxy = new ProxyServer(socket,output);
                    Thread thread = new Thread(webproxy.runProxy);
                    thread.Start();
                }
                catch(Exception e)
                {
                    e.ToString();
                }
            }
        }
        public void stop()
        {
            tcplistener.Stop();
            isActive = false;
        }

        public void Update()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)delegate ()
            {
                var context = Application.Current.MainWindow as MainWindow;
                context.ConsoleTextBlock.Text += _message+"\n";
            });
        }
    }
}
