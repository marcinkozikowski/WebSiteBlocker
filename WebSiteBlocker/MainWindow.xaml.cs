using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using WebSiteBlocker.IO;

namespace WebSiteBlocker
{
    public partial class MainWindow : Window
    {
        List<string> blackWebSitesList;
        private const string FILEPATH = "C:\\Users\\Dell\\Documents\\Visual Studio 2015\\Projects\\WebSiteBlocker\\WebSiteBlocker\\websites.txt";
        Thread tcpListenerThread=null;
        TcpListenerThread tcpListener;

        public MainWindow()
        {
            InitializeComponent();
            blackWebSitesList = new List<string>();
            try
            {
                readWriteFile("read", FILEPATH);
                setListViewItems();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Błąd wczytywania listy zakazanych stron.", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void readWriteFile(string operation,string filePath) 
        {
            FileReaderWriter file = new FileReaderWriter(filePath,blackWebSitesList);
            if(operation =="read")
            {
                blackWebSitesList =  file.readFromFile();
            }
            else if(operation=="write")
            {
                file.writeToFile(blackWebSitesList);
            }
        }

        private void setListViewItems()
        {
            foreach(string s in blackWebSitesList)
            {
                WebSitesListView.Items.Add(s);
            }
        }

        private void EditListViewItem(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Edycja danej strony www");
        }

        private void StartProxyClick(object sender, RoutedEventArgs e)
        {
            int port = 8889;

            if (tcpListenerThread != null && !tcpListenerThread.IsAlive)
            {
                tcpListenerThread.Start();
            }
            else if(tcpListenerThread == null)
            {
                tcpListener = new TcpListenerThread(port);
                tcpListenerThread = new Thread(tcpListener.run);
                tcpListenerThread.Start();
            }

        }

        private void StopProxyClick(object sender, RoutedEventArgs e)
        {
            if (tcpListenerThread.IsAlive && tcpListenerThread !=null)
            {
                tcpListener.stop();
                tcpListenerThread = null;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
