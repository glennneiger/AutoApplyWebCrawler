using Newtonsoft.Json;
using System;
using System.IO;

namespace AutoApply
{
    class Program
    {
        static void Main(string[] args)
        {
            Config config = new Config();

            // Load Configuration File for us in application
            using (StreamReader r = new StreamReader(@"..\..\config.json"))
            {
                string json = r.ReadToEnd();
                config = JsonConvert.DeserializeObject<Config>(json);
            }

            while (true)
            {
                foreach(var t in config.Terms)
                {
                    Console.WriteLine("*****************************");
                    Console.WriteLine("Search Term Used: " + t);
                    Console.WriteLine("*****************************");

                    foreach(var l in config.Locations)
                    {
                        Console.WriteLine("*****************************");
                        Console.WriteLine("Country Code: " + l.CountryCode);
                        Console.WriteLine("*****************************");

                        Apply.ViaIndeed(t, l.Location, l.CountryCode, true, config.SqlCon, config.User, config.IndeedKey);
                    }
                }

                //foreach (var s in searchTerms)
                //{

                //
                //    foreach (var c in countries)
                //    {
                //        
                //
                //        Apply.ViaIndeed(s, c.Location, c.CountryCode, true, c.CheckLang, Config.Session);
                //    }
                //}
            }

        }
       
    }
}
