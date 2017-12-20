USE CrossMyLoss

GO

-- *** Insert your information as you would like to apply for jobs *** --

INSERT INTO Users VALUES
(	
	'Your Email Address', 
	'Your Full Name', 
	'Your Phone Number (e.g. +1(703)989-8381)',
	'Full Filepath to Resume (e.g. C:\Users\PCName\Resumes\SampleCV.pdf)',
	'Full Filepath Supporting Document 1', -- May be left NULL
	'Full Filepath Supporting Document 2', -- May be left NULL
	'Full Filepath Supporting Document 3', -- May be left NULL
	'Full Filepath Supporting Document 4', -- May be left NULL
	'Full Filepath Supporting Document 5', -- May be left NULL
	'Full path to Cover Letter (e.g. C:\Users\PCName\CoverLetters\cover-letter-example.txt)'
)

-- Insert What Search Terms You would like to Use.
-- Values in here are what are used to query Indeed API to return results for a user for applications.

INSERT INTO SearchTerms VALUES
('Term 1 (e.g. "Developer")', 1),		-- Change Me
('Term 2 (e.g. ".Net Core")', 1),		-- Change Me 
('Term 3 (e.g. "Bluemix Watson")', 1)	-- Change Me
-- etc.

-- Insert values to map Users to Countries
-- You should be able to view all countries available by running 
-- "SELECT * FROM IndeedAvailCountries" at this point

-- NOTE: Location can be left as a blank empty string to apply to all Cities/Locations within a country
INSERT INTO UsersCountriesToApply VALUES
(
(SELECT AvailCountryId FROM IndeedAvailCountries WHERE CountryCode = 'us'), -- Apply to country US
'Location (e.g. New York)', -- Where location set to New York
1 -- For this user
),
(
(SELECT AvailCountryId FROM IndeedAvailCountries WHERE CountryCode = 'au'), -- Apply to country Australia
'Location (e.g. Sydney)', -- Where location set to Sydney
1 -- For this user
),
(
(SELECT AvailCountryId FROM IndeedAvailCountries WHERE CountryCode = 'gb'), -- Apply to country UK
'Location (e.g. London)', -- Where location set to London
1 -- For this user
)
--etc


-- Finally create your first session in Sessions table! (A session is a period of time for which a user applies. Helps keep track of companies we have applied to for a given period of time. I don't want to apply to the same company many times within a session which is mostly the point of this table):

INSERT INTO Sessions VALUES
(
	(SELECT GETDATE()), 
	1,
	1,
	NULL
)