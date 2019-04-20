using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;

namespace GS_MonitoringSite
{
    class Site
    {
        public string url { get; set; }
        public string validUrl { get; set; }
        public bool status { get; set; }
        

        public Site(string name)
        {
            url = name;
            CheckUrl();
            CheckStatus();
        }

        public void CheckStatus()
        {
            CheckUrl();

            if (validUrl == "Valid")
            {
                WebRequest request = WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                this.status = (response != null && response.StatusCode == HttpStatusCode.OK);
                response.Close();
            }
        }

        public void CheckUrl()
        {
            Uri uriResult;
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.SchemeDelimiter || uriResult.Scheme == Uri.UriSchemeHttps || uriResult.Scheme == Uri.UriSchemeHttp);
            validUrl = (result) ? "Valid" : "Not valid";
            status = (validUrl == "Valid") ? false : status;
        }


    }
    /*
     * Работает с сайтами из файла
     */
    class SitesFile
    {
        private string fileName = "nameSites.txt";
        private List<Site> sitesList = new List<Site>();
        private Timer pingTimer;


        public SitesFile()
        {
            CheckFileExists();
            CreateListSite();
            DefineTimer();
        }

        private void DefineTimer()
        {
            int perid = 10000;
            pingTimer = new Timer(new TimerCallback(CheckSitesByTimer), null, 0, perid);
        }

        private void CheckSitesByTimer(object obj)
        {
            CheckSites();
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
