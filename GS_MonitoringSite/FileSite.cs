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
        public bool access { get; set; }

        public Site(string name)
        {
            url = name;
            CheckUrl();
            CheckStatus();
        }

        public void CheckStatus()
        {
            if (CheckUrl())
            {
                try
                {
                    WebRequest request = WebRequest.Create(url);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    this.access = (response != null && response.StatusCode == HttpStatusCode.OK);
                    response.Close();
                }
                catch (System.Net.WebException e)
                {
                    this.access = false;
                }
            }
        }

        private bool CheckUrl()
        {
            Uri uriResult;
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.SchemeDelimiter || uriResult.Scheme == Uri.UriSchemeHttps || uriResult.Scheme == Uri.UriSchemeHttp);
            validUrl = (result) ? "Valid" : "Not valid";
            access = (validUrl == "Valid") ? false : access;

            return (validUrl == "Valid");
        }

        public static bool operator ==(Site var1, Site var2)
        {
            return (var1.url == var2.url);
        }

        public static bool operator !=(Site var1, Site var2)
        {
            return !(var1 == var2);
        }

        public override bool Equals(object obj)
        {
            return this == (Site)obj;
        }
    }
    /*
     * Работает с сайтами из файла
     */
    class SitesFile
    {
        private string fileName = "nameSites.txt";
        private List<Site> sitesList = new List<Site>();
        private int timerPeriod = 1000;
        private static object _lock = new object();

        public SitesFile()
        {
            CheckFileExists();
            CreateListSite();
            DefineTimer();
        }

        public void Add(string NewSite)
        {
            if (!sitesList.Contains(new Site(NewSite)))
            {
                lock (_lock)
                {
                    sitesList.Add(new Site(NewSite));
                }
                UpdateFileSites();
            }
        }

        public void Delete(int index)
        {
            if (sitesList.Count() >= index && index >= 0)
            {
                lock (_lock)
                {
                    sitesList.RemoveAt(index);
                }
                UpdateFileSites();
            }
        }

        private void DefineTimer()
        {
           new Timer(new TimerCallback(CheckSitesByTimer), null, 0, timerPeriod);
        }

        private void CheckSitesByTimer(object obj)
        {
           new Thread(() => CheckSites()).Start();
        }

        public ref List<Site> getSitesList()
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
                        lock (_lock)
                        {
                            sitesList.Add(new Site(lineFromFile));
                        }
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
                }
            }
            catch
            {
                Console.WriteLine("Error write to a file.");
            }
        }

        public void CheckSites()
        {
            lock (_lock)
            {
                sitesList.ForEach(site => site.CheckStatus());
            }
        }
    }
}
