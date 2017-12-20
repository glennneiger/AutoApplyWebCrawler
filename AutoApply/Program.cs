using Newtonsoft.Json;
using System;
using System.IO;

namespace AutoApply
{
    class Program
    {
        static void Main(string[] args)
        {   
            // Load Configuration File for us in application
            using (StreamReader r = new StreamReader(@"..\..\config.json"))
            {
                string json = r.ReadToEnd();
                ConfigTmp tmp = JsonConvert.DeserializeObject<ConfigTmp>(json);

                Config.DetectLanguageApiKey = tmp.DetectLanguageApiKey;
                Config.IndeedPublisherApiKey = tmp.IndeedPublisherApiKey;
                Config.SqlConnectionString = tmp.SqlConnectionString;
                Config.Session = tmp.Session;
            }

            while (true)
            {
                var countries = IndeedSql.GetCountrySearches(Config.Session);
                var searchTerms = IndeedSql.GetSearchTerms(Config.Session);

                foreach (var s in searchTerms)
                {
                    Console.WriteLine("*****************************");
                    Console.WriteLine("Search Term Used: " + s);
                    Console.WriteLine("*****************************");

                    foreach (var c in countries)
                    {
                        Console.WriteLine("*****************************");
                        Console.WriteLine("Country Applying: " + c.Country);
                        Console.WriteLine("*****************************");

                        Apply.ViaIndeed(s, c.Location, c.CountryCode, true, c.CheckLang, Config.Session);
                    }
                }
            }

        }
       
    }
}
