﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleSocketProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPool.SetMaxThreads(1000, 500);
            ThreadPool.SetMinThreads(500, 250);

            TcpListener listener = new TcpListener(IPAddress.Any, 8282);
            listener.Start();

            while (true)
            {
                Socket client = listener.AcceptSocket();
                ThreadPool.QueueUserWorkItem(ProcessSocket, client);
            }
        }

        private static readonly string patternHostPort = @"(Host:\s)(\S+)(:)(\d+)";
        private static readonly string patternHost = @"(Host:\s)(\S+)";
        private static Regex regexHostPort = new Regex(patternHostPort);
        private static Regex regexHost = new Regex(patternHost);

        static void ProcessSocket(object request)
        {
            string requestString = string.Empty;
            MemoryStream mStream = new MemoryStream();
            int bytesReceived;
            int bytesSended;
            byte[] buffer;
            byte[] byteOriginalRequest;

            Socket socketClient = (Socket)request;
            Console.WriteLine("Incoming connection: " + socketClient.RemoteEndPoint.ToString());

            buffer = new byte[4096];

            bytesReceived = socketClient.Receive(buffer, 0, buffer.Length, SocketFlags.None);
            mStream.Write(buffer, 0, bytesReceived);
            while (socketClient.Available > 0)
            {
                bytesReceived = socketClient.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                mStream.Write(buffer, 0, bytesReceived);
            }

            mStream.Close();

            byteOriginalRequest = mStream.ToArray();
            requestString = Encoding.ASCII.GetString(byteOriginalRequest);
            //Console.WriteLine(requestString);

            #region Get requested Host and Port
            string srvHost = string.Empty;
            string srvPort = string.Empty;

            Match matchHostPort = regexHostPort.Match(requestString);
            if (matchHostPort.Success)
            {
                srvHost = matchHostPort.Groups[2].Value;
                srvPort = matchHostPort.Groups[4].Value;
            }
            else
            {
                Match matchHost = regexHost.Match(requestString);
                if (matchHost.Success)
                {
                    srvHost = matchHost.Groups[2].Value;
                    srvPort = "80";
                }
                else
                {
                    Console.WriteLine("Invalid request?");
                }
            }
            #endregion

            Console.WriteLine(string.Format("Request to {0} on port {1}", srvHost, srvPort));

            IPAddress[] ipAddress = Dns.GetHostAddresses(srvHost);
            IPEndPoint endPoint = new IPEndPoint(ipAddress[0], int.Parse(srvPort));

            using (Socket socketProxy = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
            {
                try
                {
                    socketProxy.Connect(endPoint);
                

                bytesSended = socketProxy.Send(byteOriginalRequest, byteOriginalRequest.Length, SocketFlags.None);
                
                MemoryStream m2Stream = new MemoryStream();
                bytesReceived = 1;
                while (bytesReceived > 0)
                {
                    bytesReceived = socketProxy.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                    m2Stream.Write(buffer, 0, bytesReceived);
                }

                m2Stream.Close();
                byte[] finalResponse = m2Stream.ToArray();
                string stringFinalResponse = Encoding.ASCII.GetString(finalResponse);

                bytesSended = socketClient.Send(finalResponse, finalResponse.Length, SocketFlags.None);

                socketProxy.Close();
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }

            socketClient.Close();
        }
    }
}
