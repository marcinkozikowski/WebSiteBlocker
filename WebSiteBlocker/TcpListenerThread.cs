using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSiteBlocker
{
    class TcpListenerThread
    {
        int port = 8889;
        bool isActive = false;
        TcpListener tcplistener;
        public TcpListenerThread(int portN)
        {
            port = portN;
            isActive = true;
        }
        public TcpListenerThread()
        {

        }
        public void run()
        {
            
            tcplistener = new TcpListener(port);
            Console.WriteLine("Listening on port {0}", +port);
            tcplistener.Start();
            while (isActive)
            {
                try
                {
                    Socket socket = tcplistener.AcceptSocket();
                    ProxyServer webproxy = new ProxyServer(socket);
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
    }
}
