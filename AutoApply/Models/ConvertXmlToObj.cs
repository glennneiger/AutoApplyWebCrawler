using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Job_Applications_US.Bespoke
{
    class ConvertXmlToObj
    {
        public static List<Indeed> Indeed(XmlNodeList results)
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
}
