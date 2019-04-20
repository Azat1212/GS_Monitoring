using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace GS_MonitoringSite
{
    class Site
    {
        public string url { get; set; }
        public bool status { get; set; }

        public Site(string name)
        {
            url = name;
            CheckStatus();
        }

        public void CheckStatus()
        {
            WebRequest request = WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            status = (response != null && response.StatusCode == HttpStatusCode.OK);
            response.Close();
        }


    }
    /*
     * Работает с сайтами из файла
     */
    class SitesFile
    {
        private string fileName = "nameSites.txt";
        private List<Site> sitesList = new List<Site>();

        public SitesFile()
        {
            CheckFileExists();
            CreateListSite();
        }

        public ref List<Site> getFileList()
        {
            return ref sitesList;
        }
        

        private void CreateListSite()
        {
            var lineFromFile = String.Empty;

            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    while (!sr.EndOfStream)
                    {

                        lineFromFile = sr.ReadLine();
                        sitesList.Add(new Site(lineFromFile));

                    }

                }
            }
            catch
            {
                Console.WriteLine("Error file access.");
            }
        }

        private void CheckFileExists()
        {
            if (!File.Exists(fileName))
            {
                File.Create(fileName);
            }
        }

        public void UpdateFileSites()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(fileName, false))
                {
        
                    foreach (var item in sitesList)
                    {
                        sw.WriteLine(item.url);
                    }

                    if (sitesList.Count == 0)
                    {
                        sw.WriteLine("Nothing not found.");
                    }
                    sw.WriteLine();
                }
            }
            catch
            {
                Console.WriteLine("Error write to a file.");
            }


        }

        public void CheckSites()
        {
            foreach (var site in sitesList)
            {
                site.CheckStatus();
            }
        }
    }
}
