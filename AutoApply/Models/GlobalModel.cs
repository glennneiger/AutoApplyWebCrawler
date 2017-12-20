namespace AutoApply
{
    // Consistent accross Entire Application
    public static class Config
    {
        // Connection string to SQL Server, refer to README.md and Setting up SQL Server
        public static string SqlConnectionString    { get; set; }
        // To Use, get an API key from: https://www.indeed.com/publisher
        public static string IndeedPublisherApiKey  { get; set; }
        // To Use, get an API key from: http://ws.detectlanguage.com
        public static string DetectLanguageApiKey   { get; set; }
    }

    // Temp object used for Deserializing from JSON. 
    // Should match exactly with above apart from using Static memory
    public class ConfigTmp
    {
        public string SqlConnectionString   { get; set; }
        public string IndeedPublisherApiKey { get; set; }
        public string DetectLanguageApiKey  { get; set; }
    }
}
