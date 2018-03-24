using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebSiteBlocker.IO;

namespace WebSiteBlocker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> blackWebSitesList;
        private const string FILEPATH = "C:\\Users\\Dell\\Documents\\Visual Studio 2015\\Projects\\WebSiteBlocker\\WebSiteBlocker\\websites.txt";
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

    }
}
