using Newtonsoft.Json;
using System.Collections.Generic;

namespace AutoApply
{
    // Temp object used for Deserializing from JSON. 
    // Should match exactly with above apart from using Static memory
    public class Config
    {
        public Config()
        {
            Locations = new List<UserCountryApply>();
            Terms = new List<string>();
        }

        [JsonProperty("sql")]
        public string SqlCon   { get; set; }    // Connection string to SQL Server, refer to README.md and Setting up SQL Server
        [JsonProperty("indeed-api")]
        public string IndeedKey { get; set; }   // To Use, get an API key from: https://www.indeed.com/publisher
        [JsonProperty("locations")]
        public List<UserCountryApply> Locations { get; set; }   
        [JsonProperty("user")]
        public User User { get; set; }
        [JsonProperty("terms")]
        public List<string> Terms { get; set; }
    }

    // Which countries user is to apply to
    public class UserCountryApply
    {
        // Syntax as expected by Indeed API GET paramater
        [JsonProperty("country-code")]
        public string CountryCode { get; set; }

        // Location to query within Indeed API Query
        [JsonProperty("city")]
        public string Location { get; set; }
    }

    // Configuration Details for the user entered in the Indeed Application area
    public class User
    {
        [JsonProperty("email")]
        public string AppEmail { get; set; }
        [JsonProperty("full-name")]
        public string AppName { get; set; }
        [JsonProperty("phone-num")]
        public string AppPhone { get; set; }
        [JsonProperty("resume-path")]
        public string AppResumePath { get; set; }
        [JsonProperty("supporting-file1")]
        public string AppSupportingDoc1 { get; set; }
        [JsonProperty("supporting-file2")]
        public string AppSupportingDoc2 { get; set; }
        [JsonProperty("supporting-file3")]
        public string AppSupportingDoc3 { get; set; }
        [JsonProperty("supporting-file4")]
        public string AppSupportingDoc4 { get; set; }
        [JsonProperty("supporting-file5")]
        public string AppSupportingDoc5 { get; set; }
        [JsonProperty("cover-letter")]
        public string CoverLetter { get; set; }
    }
}
