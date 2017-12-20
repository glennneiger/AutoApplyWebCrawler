using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Xml;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Data.SqlClient;
using System.Data;
using System.Web;

namespace AutoApply
{
    public static class Apply
    {
        private static int count = 0;

        private static string display = string.Empty;
        
        public static void ViaIndeed(string searchTerm, string location, string countryCode, bool companySingleApply, bool checkLang, int sessionId)
        {
            // Returns Null if Session is invalid
            var user = IndeedSql.GetUserInfo(sessionId);

            // Exit if SessionId is not valid
            if (user == null)
            {
                Console.WriteLine("Session is invalid and does not exist. Please pass a SessionId that is valid.");
                return;
            }

            // Append string with rest of display
            display = "Location: " + location + " Search Term(s): " + searchTerm;

            Console.Clear();
            // Retrieve all pages with "Quick Apply" button on them ONLY
            var supersetQuickies = GetAllQuickies(searchTerm, location, countryCode, sessionId);
            Console.Clear();
            Console.WriteLine(display);

            // Reduce by configurations passed. You can reduce
            // by Language is English Only (checkLang) and/or Do not appy to 
            // same company 2ce in a session (companySingleApply)
            var reducedQuckies = ReduceQuickies(supersetQuickies, companySingleApply, checkLang, sessionId);

            //There is nothing (new) to add therefore exit
            if (reducedQuckies.Count == 0) 
                return;

            // Initialize Chrome Driver with Selenium API
            var driver = new ChromeDriver();

            for (int j = 0; j < reducedQuckies.Count; j++)
            {
                // If already applied to company proceed loop
                if (IndeedSql.AlreadyAppliedCompany(reducedQuckies[j].Company, sessionId))
                    continue;

                //Load the Job Posting page
                driver.Url = reducedQuckies[j].Url; 

                string jk = reducedQuckies[j].Jobkey; // Passed from Indeed API query
                string jobTitle = reducedQuckies[j].JobTitle; // Passed from Indeed API query
                string hl = "en"; // English parameter
                string co = countryCode; 

                //Get Variables to Store later from Job Posting page directly:
                string jobCompany = driver.FindElementByClassName("company").Text;
                string jobId = "";
                try
                {
                    jobId = driver.FindElements
                            (By.CssSelector(".indeed-apply-widget"))[0]
                            .GetAttribute("data-indeed-apply-jobid");
                }
                catch { continue; }

                string apiToken =
                    driver.FindElements(By.CssSelector(".indeed-apply-widget"))[0].GetAttribute(
                        "data-indeed-apply-apitoken");
                string postUrl =
                    driver.FindElements(By.CssSelector(".indeed-apply-widget"))[0].GetAttribute(
                        "data-indeed-apply-posturl");
                string jobUrl =
                    driver.FindElements(By.CssSelector(".indeed-apply-widget"))[0].GetAttribute(
                        "data-indeed-apply-joburl");
                string iaUid =
                    driver.FindElement(By.Id("indeed-apply-js"))
                        .GetAttribute("data-indeed-apply-qs")
                        .Replace("vjtk=", ""); //iaUid == vjtk (variable)!!!!!!
                string advNum =
                    driver.FindElements(By.CssSelector(".indeed-apply-widget"))[0].GetAttribute(
                        "data-indeed-apply-advnum");

                string email = string.Empty;

                try
                {
                    email =
                        driver.FindElements(By.CssSelector(".indeed-apply-widget"))[0].GetAttribute(
                            "data-indeed-apply-email");
                }
                catch {} 

                // Generate URL is for the Application page ONLY
                string applyUrl = GenerateURL.ApplyUrl(email, postUrl, jk, jobTitle, jobUrl, jobCompany,
                    jobId, apiToken, co, hl, advNum, iaUid);

                // Cover Letter Construction 
                string pathToTemplate = user.CoverLetter; // Path to cover letter template.
                
                // Location of template
                string coverLetter = File.ReadAllText(pathToTemplate);

                // Simple replace to insert Location on the Fly (e.g. Richmond, VA)
                coverLetter = coverLetter.Replace("%varArea%", reducedQuckies[j].FormattedLocationFull);
                
                // Simple replace to insert Position on the Fly
                coverLetter = coverLetter.Replace("%varPos%", reducedQuckies[j].JobTitle);

                // Simple replace to insert Company Name (e.g. Bit Byte Buffer, LLC)
                coverLetter = coverLetter.Replace("%varCompany%", reducedQuckies[j].Company);

                // Workaround because driver will crash if you use for too long.
                // This may be resolved with newer versions of the driver
                count++;
                if (count == 10)
                {
                    count = 0;
                    driver.Close();
                    driver = new ChromeDriver();
                    driver.Url = reducedQuckies[j].Url; //Information Page
                }

                // Navigate to Application Page
                driver.Url = applyUrl;

                // Fill in My Name 
                try // 1st Potential Id
                {
                    driver.FindElementById("applicant.name").Clear();
                    driver.FindElementById("applicant.name").SendKeys(user.AppName);
                }
                catch
                {
                    try // 2nd Potential Id (prefixed with "input-")
                    {
                        driver.FindElementById("input-applicant.name").Clear();
                        driver.FindElementById("input-applicant.name").SendKeys(user.AppName);
                    }
                    catch 
                    {
                        // Can't insert name, therefore must not be a 
                        // quick apply (otherwise run should go smoothly)
                        continue;
                    }
                }

                // Fill in My Email
                try // 1st Potential Id
                {
                    driver.FindElementById("applicant.email").Clear();
                    driver.FindElementById("applicant.email").SendKeys(user.AppEmail);
                }
                catch
                {
                    try // 2nd Potential Id (prefixed with "input-")
                    {
                        driver.FindElementById("input-applicant.email").Clear();
                        driver.FindElementById("input-applicant.email").SendKeys(user.AppEmail);
                    }
                    catch
                    {
                        continue;
                    } 
                }

                // Fill in Phone Number
                try // 1st Potential Id
                {
                    driver.FindElementById("applicant.phoneNumber").Clear();
                    driver.FindElementById("applicant.phoneNumber").SendKeys(user.AppPhone);
                }
                catch
                {
                    try // 2nd Potential Id (prefixed with "input-")
                    {
                        driver.FindElementById("input-applicant.phoneNumber").Clear();
                        driver.FindElementById("input-applicant.phoneNumber").SendKeys(user.AppPhone);
                    }
                    catch
                    {
                        continue;
                    }
                }

                //Upload Resume
                try 
                {
                    driver.FindElementById("resume")
                        .SendKeys(user.AppResumePath);
                }
                catch
                {
                    try
                    {
                        driver.FindElementById("ia-FilePicker-resume")
                        .SendKeys(user.AppResumePath);
                    }
                    catch
                    {
                        continue;
                    }
                }

                //Upload Supporting documents (e.g. Degree, Certifications, etc.) 
                try
                {
                    if (!string.IsNullOrEmpty(user.AppSupportingDoc1))
                    {
                        driver.FindElementById("multattach1")
                        .SendKeys(user.AppSupportingDoc1);
                    }

                    if (!string.IsNullOrEmpty(user.AppSupportingDoc2))
                    {
                        driver.FindElementById("multattach2")
                        .SendKeys(user.AppSupportingDoc2);
                    }

                    if (!string.IsNullOrEmpty(user.AppSupportingDoc3))
                    {
                        driver.FindElementById("multattach3")
                        .SendKeys(user.AppSupportingDoc3);
                    }

                    if (!string.IsNullOrEmpty(user.AppSupportingDoc4))
                    {
                        driver.FindElementById("multattach4")
                        .SendKeys(user.AppSupportingDoc4);
                    }

                    if (!string.IsNullOrEmpty(user.AppSupportingDoc5))
                    {
                        driver.FindElementById("multattach5")
                        .SendKeys(user.AppSupportingDoc5);
                    }
                }
                catch
                {
                    // stub: means there is no control for 
                    // supporting file uploads on page which is common
                }

                try
                {
                    // Fill cover letter text box on form (attempt 1 with original Id)

                    IJavaScriptExecutor js = driver;
                    js.ExecuteScript("document.getElementById('applicant.applicationMessage').setAttribute('style', '')");
                    driver.FindElementById("applicant.applicationMessage").Click();
                    driver.FindElementById("applicant.applicationMessage").Clear(); 
                    driver.FindElementById("applicant.applicationMessage").SendKeys(coverLetter);

                }
                catch
                {
                    try
                    {
                        // Fill cover letter text box on form (attempt 2 with new preixed "textarea-" Id)

                        IJavaScriptExecutor js = driver;
                        js.ExecuteScript("document.getElementById('textarea-applicant.applicationMessage').setAttribute('style', '')");
                        driver.FindElementById("textarea-applicant.applicationMessage").Click();
                        driver.FindElementById("textarea-applicant.applicationMessage").Clear(); 
                        driver.FindElementById("textarea-applicant.applicationMessage").SendKeys(coverLetter);
                    }
                    catch
                    {
                        // Could not identify an Application Message textbox to fill
                        continue;
                    }
                }
                //************ Apply Here *************

                //Click on button submit

                try // Attempt 1, click button by CSS Class Selector
                { 
                    driver.FindElement(By.CssSelector(".button_content")).Click();
                }
                catch
                {
                    try // Attempt 2, click button by CSS ID Selector
                    {
                        driver.FindElement(By.CssSelector("#form-action-submit")).Click();
                    }
                    catch
                    {
                        // Stub: Cannot find Application Button
                        continue;
                    }
                }

                // Let new page load for 1.5 seconds after clicking 
                // submit application button
                Thread.Sleep(1500);

                try // Additional Yes/No questions added to US applications sometimes
                {
                    driver.FindElement(By.Id("q_0")).SendKeys("Yes");
                    driver.FindElement(By.Id("apply")).Click();
                }
                catch
                {
                    // Stub: Not uncommon for this not to exist. In most cases it does not exist.
                }

                // Start Save process
                Stopwatch sw = new Stopwatch();
                sw.Start();

                while (true)
                {
                    try
                    {
                        //Give it 2.5 minutes to upload all documents as max time permissible
                        if (sw.ElapsedMilliseconds > 60000 * 2.5) 
                        {
                            // Insert Failure message here!!
                            SaveSubmission(reducedQuckies[j], false, sessionId); //Changed to true as they are fine.
                            Console.Clear();
                            Console.WriteLine(display);
                            Console.WriteLine("\nFailed to apply: " + reducedQuckies[j].Company + " (" + reducedQuckies[j].JobTitle + ")");
                            break; // Exit while loop
                        }

                        driver.FindElement(By.Id("ia_success"));
                        //Insert success message here!!
                        Console.Clear();
                        Console.WriteLine(display);
                        Console.WriteLine("\nSucceeded in applying to: " + reducedQuckies[j].Company + " (" + reducedQuckies[j].JobTitle + ")");
                        SaveSubmission(reducedQuckies[j], true, sessionId);
                        break;

                    }
                    catch
                    {
                        // Stub: recursion occurs until element found on page
                        // and save is successful, or element is not found after
                        // 2.5 minutes and record is saved as unsuccessful application
                    }
                }

                // Sleep for 2 seconds before moving to next application
                Thread.Sleep(2000);
            }

                driver.Close();
                driver.Dispose();

            }

        // ********************************************************* Private Methods

        private static List<Indeed> ReduceQuickies(List<Indeed> superset, bool companySingleApply, bool checkLang, int sessionId)
        {

            List<Indeed> reduced = new List<Indeed>();

            if (companySingleApply && checkLang)
            {
                reduced.AddRange
                (
                    superset.Where
                    (
                        page =>
                        !IndeedSql.AlreadyAppliedCompany(page.Company, sessionId) &&
                        !IndeedSql.AlreadyAppliedToJob(page.Jobkey, page.Url, sessionId) &&
                        DetectLanguage.IsEnglish(page.Snippet)
                    )
                );
            }
            else if (companySingleApply && !checkLang)
            {
                reduced.AddRange
                (
                    superset.Where
                    (
                        page =>
                        !IndeedSql.AlreadyAppliedCompany(page.Company, sessionId) &&
                        !IndeedSql.AlreadyAppliedToJob(page.Jobkey, page.Url, sessionId)
                    )
                );
            }
            else
            {
                reduced.AddRange
                    (
                        superset.Where
                        (
                            page => !IndeedSql.AlreadyAppliedToJob(page.Jobkey, page.Url, sessionId)
                        )
                    );
            }

            return reduced;
        }

        private static bool SaveSubmission(Indeed applied, bool successfulApply, int sessionId)
        {
            InsertApplyOps ops = new InsertApplyOps();

            ops.Jobkey = applied.Jobkey ?? "";
            ops.Url = applied.Url ?? "";
            ops.Snippet = applied.Snippet ?? "";
            ops.JobTitle = applied.JobTitle ?? "";
            ops.Company = applied.Company ?? "";
            ops.DateTimeApplied = DateTime.Now;
            ops.Sponsored = applied.Sponsored.ToString();
            ops.Expired = applied.Expired.ToString();
            ops.IndeedApply = applied.IndeedApply.ToString();
            ops.FormattedLocationFull = applied.FormattedLocationFull ?? "";
            ops.FormattedRelativeTime = applied.FormattedRelativeTime ?? "";
            ops.OnMouseDown = applied.OnMouseDown ?? "";

            ops.Latitude = applied.Latitude ?? "";
            ops.Longitude = applied.Longitude ?? "";
            ops.City = applied.City ?? "";
            ops.State = applied.State ?? "";
            ops.Country = applied.Country ?? "";
            ops.FormattedLocation = applied.FormattedLocation ?? "";
            ops.Source = applied.Source ?? "";
            ops.Date = applied.Date ?? "";

            try
            {
                IndeedSql.InsertApply(ops, successfulApply, sessionId);
                return true;
            }
            catch
            {
                Console.WriteLine("ERROR IN SAVING TO DB: MANUALLY INSERT NOW");
                return false;
            }
        }

        private static XmlDocument ConsumeWebServiceRest(string url)
        {
            WebClient client = new WebClient();

            string xml = client.DownloadString(url); //Gets all Elements on page

            client.Dispose(); //Release from memory as only used to get page content string (in this case XML as it's a web service)

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(xml); //Convert string to XML Doc (as this is a REST web service)

            return doc;
        }

        private static List<Indeed> GetAllQuickies(string searchTerm, string location, string country, int sessionId)
        {
            // Start position (startFromPosition) set to 0, meaning 1st Search Result page. 
            string url = GenerateURL.Search(searchTerm, location, 0, country);
            
            //Get number of times to loop --
            XmlDocument xmlDocument = ConsumeWebServiceRest(url);
            int numResults = xmlDocument.GetElementsByTagName("totalresults").Item(0) == null ? 0 : int.Parse(xmlDocument.GetElementsByTagName("totalresults").Item(0).InnerXml);
            XmlResult.TotalResults = numResults;
            int timesToIterate = (int)Math.Ceiling((decimal)XmlResult.TotalResults / 25); //Round up no matter what i.e. Math.Ceiling
                                                                                                 //Get number of times to loop --

            //Get Everything from Web Service Query --
            var quickies = new List<Indeed>(); // quick application pages

            Stopwatch totalTime = new Stopwatch();
            totalTime.Start();
            for (int i = 0; i < timesToIterate; i++)
            {
                Console.Clear();
                Console.WriteLine(display + "\nLoading page... " + (i + 1) + "/" + timesToIterate);

                int pageStartFromRecord = i * 25; //25 records per page, way of receiving all records for current page
                url = GenerateURL.Search(searchTerm, location, pageStartFromRecord, country);
                xmlDocument = ConsumeWebServiceRest(url);
                XmlNodeList results = xmlDocument.GetElementsByTagName("result");
                var batchIndeeds = ConvertXmlToObj(results);

                //Which are quickies && Is Snippet in English?
                foreach (var indeed in batchIndeeds)
                {   
                    if (indeed.IndeedApply) //If document consists of quick apply button, add to quicky list
                    {
                        quickies.Add(indeed); //Indeed - Quick Apply Application add to list   
                    }

                }
                //Which are quickies?

                // Define Quicky: A page returned by Indeed API that consists a "Quick Apply" button on it.

            }
            //Get Everything from Web Service Query --

            return quickies;
        }

        private static List<Indeed> ConvertXmlToObj(XmlNodeList results)
        {
            List<Indeed> li = new List<Indeed>();

            foreach (XmlNode n in results)
            {
                Indeed i = new Indeed();

                foreach (XmlNode n2 in n)
                {
                    switch (n2.Name)
                    {
                        case "jobtitle":
                            i.JobTitle = n2.InnerXml;
                            break;
                        case "company":
                            i.Company = n2.InnerXml;
                            break;
                        case "city":
                            i.City = n2.InnerXml;
                            break;
                        case "state":
                            i.State = n2.InnerXml;
                            break;
                        case "country":
                            i.Country = n2.InnerXml;
                            break;
                        case "formattedLocation":
                            i.FormattedLocation = n2.InnerXml;
                            break;
                        case "source":
                            i.Source = n2.InnerXml;
                            break;
                        case "date":
                            i.Date = n2.InnerXml;
                            break;
                        case "snippet":
                            i.Snippet = n2.InnerXml;
                            break;
                        case "url":
                            i.Url = n2.InnerXml;
                            break;
                        case "onmousedown":
                            i.OnMouseDown = n2.InnerXml;
                            break;
                        case "latitude":
                            i.Latitude = n2.InnerXml;
                            break;
                        case "longitude":
                            i.Longitude = n2.InnerXml;
                            break;
                        case "jobkey":
                            i.Jobkey = n2.InnerXml;
                            break;
                        case "sponsored":
                            i.Sponsored = n2.InnerXml == "true";
                            break;
                        case "expired":
                            i.Expired = n2.InnerXml == "true";
                            break;
                        case "indeedApply":
                            i.IndeedApply = n2.InnerXml == "true";
                            break;
                        case "formattedLocationFull":
                            i.FormattedLocationFull = n2.InnerXml;
                            break;
                        case "formattedRelativeTime":
                            i.FormattedRelativeTime = n2.InnerXml;
                            break;
                    }
                }

                li.Add(i);
            }


            return li;
        }

    }

    internal static class IndeedSql
    {        
        static SqlConnection con = new SqlConnection(Config.SqlConnectionString);

        public static List<string> GetSearchTerms(int sessionId)
            {
                string query = "SELECT Term FROM SearchTerms WHERE UserId = " +
                               "(SELECT UserId FROM Sessions WHERE " +
                               "SessionId = " + sessionId + ")";

                SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandText = query,
                    CommandType = CommandType.Text
                };

                List<string> terms = new List<string>();

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    terms.Add(reader["Term"].ToString());
                }

                con.Close();

                return terms;
            }

        public static List<UserCountryApply> GetCountrySearches(int sessionId)
            {
                string query = "SELECT b.Country, b.CountryCode, a.LocationSearch, b.CheckLang FROM UsersCountriesToApply a INNER JOIN IndeedAvailCountries b ON a.AvailCountryId = b.AvailCountryId WHERE a.UserId = (SELECT UserId FROM Sessions WHERE SessionId = " + sessionId + ")";

                SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandText = query,
                    CommandType = CommandType.Text
                };

                List<UserCountryApply> applies = new List<UserCountryApply>();

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    applies.Add(new UserCountryApply()
                    {
                        Country = reader["Country"].ToString(),
                        CountryCode = reader["CountryCode"].ToString(),
                        Location = reader["LocationSearch"].ToString() == null ? "" : reader["LocationSearch"].ToString(),
                        CheckLang = (bool)reader["CheckLang"]
                    });
                }

                con.Close();

                return applies;
            }

        public static void InsertApply(InsertApplyOps options, bool successfulApply, int sessionId)
            {
                var success = -1;

                if (successfulApply)
                    success = 1;
                else
                    success = 0;

                string insert = "INSERT INTO ApplyIndeed (" +
                                "SuccessfulApply, " +
                                "Jobkey, " +
                                "Url, " +
                                "Snippet, " +
                                "JobTitle, " +
                                "Company, " +
                                "DateTimeApplied, " +
                                "Sponsored, " +
                                "Expired, " +
                                "IndeedApply," +
                                "FormattedLocationFull, " +
                                "FormattedRelativeTime," +
                                "OnMouseDown, " +
                                "Latitude, " +
                                "Longitude, " +
                                "City, " +
                                "State, " +
                                "Country, " +
                                "FormattedLocation, " +
                                "Source, " +
                                "Date, SessionId) VALUES (";
                insert += success + ",";
                insert += "'" + options.Jobkey.Replace("'", "''") + "',";
                insert += "'" + options.Url.Replace("'", "''") + "',";
                insert += "'" + options.Snippet.Replace("'", "''") + "',";
                insert += "'" + options.JobTitle.Replace("'", "''") + "',";
                insert += "'" + options.Company.Replace("'", "''") + "',";
                insert += "'" + options.DateTimeApplied.ToString("s", System.Globalization.CultureInfo.InvariantCulture) + "',";
                insert += "'" + options.Sponsored.Replace("'", "''") + "',";
                insert += "'" + options.Expired.Replace("'", "''") + "',";
                insert += "'" + options.IndeedApply.Replace("'", "''") + "',";
                insert += "'" + options.FormattedLocationFull.Replace("'", "''") + "',";
                insert += "'" + options.FormattedRelativeTime.Replace("'", "''") + "',";
                insert += "'" + options.OnMouseDown.Replace("'", "''") + "',";
                insert += "'" + options.Latitude.Replace("'", "''") + "',";
                insert += "'" + options.Longitude.Replace("'", "''") + "',";
                insert += "'" + options.City.Replace("'", "''") + "',";
                insert += "'" + options.State.Replace("'", "''") + "',";
                insert += "'" + options.Country.Replace("'", "''") + "',";
                insert += "'" + options.FormattedLocation.Replace("'", "''") + "',";
                insert += "'" + options.Source.Replace("'", "''") + "',";
                insert += "'" + options.Date.Replace("'", "''") + "',";
                insert += sessionId;
                insert += ")";


                SqlCommand cmd = new SqlCommand()
                {
                    CommandText = insert,
                    CommandType = CommandType.Text,
                    Connection = con
                };

                con.Open();
                try { cmd.ExecuteNonQuery(); } catch { }
                con.Close();
            }

        public static bool AlreadyAppliedToJob(string jobKey, string url, int sessionId)
            {
                //RETURN TRUE:
                //if url or jobkey exists //Previous Successful Application!

                string query = "SELECT TOP 1 * FROM ApplyIndeed a INNER JOIN Sessions b ON a.SessionId = b.SessionId WHERE (URL='" + url + "' OR Jobkey = '" + jobKey + "') AND b.SessionId = " + sessionId;

                SqlCommand cmd = new SqlCommand()
                {
                    CommandText = query,
                    CommandType = CommandType.Text,
                    Connection = con
                };

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    con.Close();
                    return true;
                }

                con.Close();
                return false;
            }

        public static bool AlreadyAppliedCompany(string company, int sessionId)
            {

                //RETURN TRUE:
                //if company was already applied to  ---  Apply to one job per company per session

                string query = "SELECT TOP 1 * FROM ApplyIndeed a INNER JOIN Sessions b ON a.SessionId = b.SessionId WHERE a.SessionId = " + sessionId + " AND a.Company = '" + company.Replace("'", "''") + "'";

                SqlCommand cmd = new SqlCommand()
                {
                    CommandText = query,
                    CommandType = CommandType.Text,
                    Connection = con
                };

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    con.Close();
                    return true;
                }

                con.Close();
                return false;

            }

        public static User GetUserInfo(int sessionId)
            {
                string query = "SELECT a.* FROM Users a INNER JOIN Sessions b ON a.UserId = b.UserId WHERE SessionId = " + sessionId;

                SqlCommand cmd = new SqlCommand()
                {
                    CommandText = query,
                    CommandType = CommandType.Text,
                    Connection = con
                };

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    var user = new User()
                    {
                        AppEmail = reader["AppEmail"].ToString(),
                        AppName = reader["AppName"].ToString(),
                        AppPhone = reader["AppPhone"].ToString(),
                        AppResumePath = reader["AppResumePath"].ToString(),
                        AppSupportingDoc1 = reader["AppSupportingDoc1"].ToString(),
                        AppSupportingDoc2 = reader["AppSupportingDoc2"].ToString(),
                        AppSupportingDoc3 = reader["AppSupportingDoc3"].ToString(),
                        AppSupportingDoc4 = reader["AppSupportingDoc4"].ToString(),
                        AppSupportingDoc5 = reader["AppSupportingDoc5"].ToString(),
                        CoverLetter = reader["CoverLetter"].ToString()
                    };

                    con.Close();

                    return user;
                }
                else
                {
                    con.Close();
                    return null;
                }

            }

    }

    internal static class GenerateURL
    {
        public static string Search(string searchTerm, string location, int startFromRecord, string country)
        {
            string url = "http://api.indeed.com/ads/apisearch?";
            url += "publisher=" + Config.IndeedPublisherApiKey; // Indeed publisher ID used to Query Indeed API
            url += "&v=2"; // Required (available 1 or 2 [suggested 2])
            url += "&format=xml"; // response to be xml (xml or json selectable) 
            url += "&q=" + HttpUtility.UrlEncode(searchTerm); //query ************
            url += "&l=" + HttpUtility.UrlEncode(location); //location
            url += "&radius="; //distance
            url += "&sort=date"; // (possible = date || relevance)
            url += "&st=employer"; // site type (possible = jobsite || employer)
            url += "&jt=fulltime"; // job type (permissible: "fulltime", "parttime", "contract", "internship", "temporary")
            url += "&start=" + startFromRecord; // Start results at this result number, beginning with 0. Default is 0.
            url += "&limit=25"; // Maximum number of results returned per query. Default is 10
            url += "&fromage=100"; //Number of days back to search (e.g. within past n[e.g. 10] days)
            url += "&highlight=0"; //Setting this value to 1 will bold terms in the snippet that are also present in q. Default is 0.
            url += "&filter=1"; //Filter duplicate results. 0 turns off duplicate job filtering. Default is 1.
            url += "&latlong=1"; //If latlong=1, returns latitude and longitude information for each job result. Default is 0.
            url += "&co=" + country; //Search within country specified. Default is us See below for a complete list of supported countries.
            url += "&chnl=name"; //Channel Name: Group API requests to a specific channel
            url += "&userip=1.2.3.4"; //+ GetMyIp(); //The IP number of the end-user to whom the job results will be displayed. This field is required.
            url += "&useragent=Chrome"; //The User-Agent (browser) of the end-user to whom the job results will be displayed. This can be obtained from the "User-Agent" HTTP request header from the end-user. This field is required.

            return url;
        }

        public static string ApplyUrl(string email, string postUrl, string jk, string jobTitle, string jobUrl, string company, string jobId, string apiToken, string country, string hl, string advNum, string iaUid)
        {
            string url = "https://apply.indeed.com/indeedapply/resumeapply?";
            url += "jk=" + jk;
            url += string.IsNullOrEmpty(email) ? "" : "&applyEmail=" + email; 
            url += string.IsNullOrEmpty(postUrl) ? "" : "&postUrl=" + postUrl; 
            url += "&jobTitle=" + HttpUtility.UrlEncode(jobTitle);
            url += "&jobUrl=" + jobUrl;
            url += "&jobCompany=" + company;
            url += "&jobId=" + jobId;
            url += "&name=fullname";
            url += "&phone=Optional";
            url += "&coverletter=Optional";
            url += "&resume=REQUIRED";
            url += "&postFormat=JSON";
            url += "&apiToken=" + apiToken;
            url += "&co=" + country;
            url += "&iaUid=" + iaUid;
            url += "&advNum=" + advNum;
            url += "&hl=" + hl;

            return url;
        }

    }

}
