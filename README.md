# Automatically Apply to Jobs Web Crawler

Application for Automatically Applying to jobs using a Web Crawler (i.e. using Selenium API). This is a simple .Net Console application for demonstrative purposes only. 

# What You Will Need

1. Windows 7 or 10 Operating System
2. Microsoft Sql Server Database
3. (Required) Api Key from https://www.indeed.com/publisher
4. (Optional) Api Key from http://ws.detectlanguage.com
5. (Optional) Visual Studio (VS 2017 Community is Free)

# Getting Started

1. Create a Sql Server Database either locally or in a cloud of your choosing which is a bit easier (e.g. AWS RDS, Azure, etc.)
2. Open and Run the SQL commands as defined in file "Create Database.sql"
3. Open and **ALTER DATA** to your personal information as described in file "Insert Your Data.sql". Once complete add the data by running the commands
4. Open Configuration file found in "AutoApply/config.json" and alter the following json object properties:
	* "DetectLanguageApiKey" with API Key as received from http://ws.detectlanguage.com or change to "demo" for a few free calls
	* "IndeedPublisherApiKey" with API key as received from https://www.indeed.com/publisher
	* "SqlConnectionString" with connection string to connect to Sql Server database we created and populated in steps 1 through 3
	* **"Session" with the SessionId you would like to Run as defined in CrossMyLoss database in table Sessions**

# Getting Finished

Run the application by doing any of the following:

1. Open in Visual Studio and hit Run/f5 while in Debug mode
2. Navigate to and double click "AutoApply/bin/Debug/AutoApply.exe"
3. Navigate to and double click "AutoApply/bin/Release/AutoApply.exe"
4. Enjoy :)
