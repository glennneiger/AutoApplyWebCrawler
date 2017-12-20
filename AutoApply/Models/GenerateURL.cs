using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Applications_US.Bespoke
{
    public static class GenerateURL
    {
        public static class Indeed
        {
            public static string FirstSearch(string searchTerm, string location)
            {
                string url = "http://api.indeed.com/ads/apisearch?";
                url += "publisher=3923842189789412"; //My personal XML query Id
                url += "&v=2"; // Required (available 1 or 2 [suggested 2])
                url += "&format=xml"; //response to be xml (xml or json selectable) 
                url += "&q=" + searchTerm; //query ************
                url += "&l=" + location; //location
                url += "&radius=100"; //distance
                url += "&sort=date"; // (possible = date || relevance)
                url += "&st=employer"; // site type (possible = jobsite || employer)
                //url += "&jt=fulltime"; // job type (permissible: "fulltime", "parttime", "contract", "internship", "temporary")
                url += "&start=0"; // Start results at this result number, beginning with 0. Default is 0.
                url += "&limit=1"; // Maximum number of results returned per query. Default is 10
                url += "&fromage=100"; //Number of days back to search (e.g. within past n[e.g. 10] days)
                url += "&highlight=0"; //Setting this value to 1 will bold terms in the snippet that are also present in q. Default is 0.
                url += "&filter=1"; //Filter duplicate results. 0 turns off duplicate job filtering. Default is 1.
                url += "&latlong=1"; //If latlong=1, returns latitude and longitude information for each job result. Default is 0.
                url += "&co=US"; //Search within country specified. Default is us See below for a complete list of supported countries.
                url += "&chnl=name"; //Channel Name: Group API requests to a specific channel
                url += "&userip=1.2.3.4"; //DUMMY WORKS. The IP number of the end-user to whom the job results will be displayed. This field is required.
                url += "&useragent=Chrome"; //The User-Agent (browser) of the end-user to whom the job results will be displayed. This can be obtained from the "User-Agent" HTTP request header from the end-user. This field is required.

                return url;
            }

            public static string Search(string searchTerm, string location, int startFromRecord)
            {
                string url = "http://api.indeed.com/ads/apisearch?";
                url += "publisher=3923842189789412"; //My personal XML query Id
                url += "&v=2"; // Required (available 1 or 2 [suggested 2])
                url += "&format=xml"; //response to be xml (xml or json selectable) 
                url += "&q=" + searchTerm; //query ************
                url += "&l=" + location; //location
                url += "&radius=100"; //distance
                url += "&sort=date"; // (possible = date || relevance)
                url += "&st=employer"; // site type (possible = jobsite || employer)
                //url += "&jt=fulltime"; // job type (permissible: "fulltime", "parttime", "contract", "internship", "temporary")
                url += "&start=" + startFromRecord; // Start results at this result number, beginning with 0. Default is 0.
                url += "&limit=25"; // Maximum number of results returned per query. Default is 10
                url += "&fromage=100"; //Number of days back to search (e.g. within past n[e.g. 10] days)
                url += "&highlight=0"; //Setting this value to 1 will bold terms in the snippet that are also present in q. Default is 0.
                url += "&filter=1"; //Filter duplicate results. 0 turns off duplicate job filtering. Default is 1.
                url += "&latlong=1"; //If latlong=1, returns latitude and longitude information for each job result. Default is 0.
                url += "&co=US"; //Search within country specified. Default is us See below for a complete list of supported countries.
                url += "&chnl=name"; //Channel Name: Group API requests to a specific channel
                url += "&userip=1.2.3.4"; //+ GetMyIp(); //The IP number of the end-user to whom the job results will be displayed. This field is required.
                url += "&useragent=Chrome"; //The User-Agent (browser) of the end-user to whom the job results will be displayed. This can be obtained from the "User-Agent" HTTP request header from the end-user. This field is required.

                return url;
            }

            public static string ApplyUrl(string email, string postUrl, string jk, string jobTitle, string jobUrl, string company, string jobId, string apiToken, string country, string hl, string advNum, string iaUid)
            {
                string url = "https://apply.indeed.com/indeedapply/resumeapply?";
                url += "jk=" + jk;
                url += string.IsNullOrEmpty(email)? "" : "&applyEmail=" + email; //either email is required
                url += string.IsNullOrEmpty(postUrl) ? "" : "&postUrl=" + postUrl; //or postUrl
                url += "&jobTitle=" + jobTitle.Replace(" ", "-");
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
}
