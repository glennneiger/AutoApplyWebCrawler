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

    // Which countries user is to apply to
    public class UserCountryApply
    {
        // To print to console for keeping track in loop
        public string Country { get; set; }

        // Syntax as expected by Indeed API GET paramater
        public string CountryCode { get; set; }

        // Location to query within Indeed API Query
        public string Location { get; set; }

        // If set to yes, the method to check if English is called. If not in English application will be skipped.
        public bool CheckLang { get; set; }
    }

    // Configuration Details for the user entered in the Indeed Application area
    public class User
    {
        public string AppEmail { get; set; }
        public string AppName { get; set; }
        public string AppPhone { get; set; }
        public string AppResumePath { get; set; }
        public string AppSupportingDoc1 { get; set; }
        public string AppSupportingDoc2 { get; set; }
        public string AppSupportingDoc3 { get; set; }
        public string AppSupportingDoc4 { get; set; }
        public string AppSupportingDoc5 { get; set; }
        public string CoverLetter { get; set; }
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
