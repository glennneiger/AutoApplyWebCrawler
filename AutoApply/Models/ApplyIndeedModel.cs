using Newtonsoft.Json;
using System;

namespace AutoApply
{

    // Returned Object from Indeed API (XML)
    class Indeed
    {
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string FormattedLocation { get; set; }
        public string Source { get; set; }
        public string Date { get; set; }
        public string Snippet { get; set; }
        public string Url { get; set; }
        public string OnMouseDown { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Jobkey { get; set; }
        public bool Sponsored { get; set; }
        public bool Expired { get; set; }
        public bool IndeedApply { get; set; }
        public string FormattedLocationFull { get; set; }
        public string FormattedRelativeTime { get; set; }
    }

    // Object inserted to Database in the case we would want this
    public class InsertApplyOps
    {
        public string Jobkey { get; set; }
        public string Url { get; set; }
        public string Snippet { get; set; }
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public DateTime DateTimeApplied { get; set; }
        public string Sponsored { get; set; }
        public string Expired { get; set; }
        public string IndeedApply { get; set; }
        public string FormattedLocationFull { get; set; }
        public string FormattedRelativeTime { get; set; }
        public string OnMouseDown { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string FormattedLocation { get; set; }
        public string Source { get; set; }
        public string Date { get; set; }

    }

    // Metadata returned by first query, used to iterate through "pages" returned and populate list of 
    // application pages with "Quick Apply" button available (API does not have mechanism to filter by 
    // "quick apply" so we need to iterate through)
    public static class XmlResult
    {
        public static string Query { get; set; }
        public static string Location { get; set; }
        public static string DupeFilter { get; set; }
        public static int TotalResults { get; set; }
    }

}
