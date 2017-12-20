using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Applications_US.Bespoke
{
    class SQL
    {
        public static class Indeed
        {
            public static void InsertApply()
            {
                SqlConnection con = new SqlConnection(@"Data Source=PC98\SQLEXPRESS;Initial Catalog=CrossMyLoss;Integrated Security=True");

                string insert = "INSERT INTO APPLY_INDEED (" +
                                "Jobkey, " +
                                "Url, " +
                                "Snippet, " +
                                "JobTitle, " +
                                "Company, " +
                                "DateApplied, " +
                                "DateTimeApplied, " +
                                "ResumeUsed, " +
                                "CoverLetter, "+
                                "PhoneNumberAppliedWith, " +
                                "AdditionalSupportingDocument," +
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
                                "Date) VALUES (";
                insert += "''";
                insert += "''";
                insert += "''";
                insert += "''";
                insert += "''";
                insert += "''";
                insert += "''";
                insert += "''";
                insert += "''";
                insert += "''";
                insert += "''";
                insert += "''";
                insert += "''";
                insert += "''";
                insert += "''";
                insert += "''";
                insert += "''";
                insert += "''";
                insert += "''";
                insert += "''";
                insert += "''";
                insert += "''";
                insert += "''";
                insert += "''";
                insert += "''";
                insert += "''";
                insert += ")";




                SqlCommand cmd = new SqlCommand()
                {
                    CommandText = insert,
                    CommandType = CommandType.Text,
                    Connection = con
                };

                con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

            }



            
        }
    }
}
