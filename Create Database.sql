CREATE DATABASE CrossMyLoss;
GO

USE [CrossMyLoss]
GO

CREATE TABLE [dbo].[ApplyIndeed]
(
	[ApplicationId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY, -- Auto Incrementing Primary Key
	[SuccessfulApply] [bit] NOT NULL, -- Indicates if something went wrong during an application run
	[Jobkey] [varchar](255) NOT NULL, -- Uniquely identifies job. Scraped from Job Posting page
	[Url] [varchar](max) NULL, -- URL of Job Posting page for reference when employers are giving you a call
	[Snippet] [varchar](255) NOT NULL, -- Short description of Job Posting
	[JobTitle] [varchar](255) NOT NULL, -- Title of position (e.g. Senior ASP.Net Developer)
	[Company] [varchar](255) NOT NULL, -- Name of the company
	[DateTimeApplied] [datetime] NOT NULL, -- Timestamp of when the APPLICATION has applied to this job posting.
	[Sponsored] [varchar](255) NOT NULL, -- I don't know what this is, but it comes from Indeed Publisher API so someone may find it useful.
	[Expired] [varchar](255) NOT NULL, -- I don't know what this is, but it comes from Indeed Publisher API so someone may find it useful.
	[IndeedApply] [varchar](255) NOT NULL, -- Will always be true 'True'. Application only applies to these job postings.
	[FormattedLocationFull] [varchar](255) NOT NULL, -- Location Formatted (Somewhat Useful, not UTF-8)
	[FormattedRelativeTime] [varchar](255) NOT NULL, -- Location Time Zone (Not very useful as not UTF-8)
	[OnMouseDown] [varchar](255) NOT NULL, -- Snippet of Code returned by Publisher API
	[Latitude] [varchar](255) NOT NULL, -- Lat
	[Longitude] [varchar](255) NOT NULL, -- Long
	[City] [varchar](255) NOT NULL, -- City
	[State] [varchar](255) NOT NULL, -- State if in US
	[Country] [varchar](255) NOT NULL, -- Country
	[FormattedLocation] [varchar](255) NOT NULL, -- Location
	[Source] [varchar](255) NOT NULL, --Usually 'Indeed', sometimes otherwise. This is where the Job Posting comes from.
	[Date] [varchar](255) NOT NULL -- Date Job Posting was put out to the world by Indeed as returned by Indeed Publisher API. 
)
