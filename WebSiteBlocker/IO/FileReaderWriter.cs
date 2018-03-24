using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSiteBlocker.IO
{
    class FileReaderWriter
    {
        StreamReader reader;
        StreamWriter writer;
        string filePath;
        List<string> webSites;

        public FileReaderWriter(string path,List<string> _webSites)
        {
            filePath = path;
            webSites = _webSites;
        }
        public List<string> readFromFile()
        {
            reader = new StreamReader(filePath);
            try
            {
                while(!reader.EndOfStream)
                {
                    webSites.Add(reader.ReadLine());
                }
            }
            catch(Exception e)
            {
                throw new IOException(e.ToString());
            }
            return webSites;
        }

        public void writeToFile(List<string> webSites)
        {
            writer = new StreamWriter(filePath);
            try
            {
                foreach (string s in webSites)
                {
                    writer.WriteLine(s);
                }
            }
            catch (Exception e)
            {
                throw new IOException(e.ToString());
            }
        }

    }
}
