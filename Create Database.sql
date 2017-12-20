CREATE DATABASE CrossMyLoss;
GO

USE [CrossMyLoss]
GO

/****** Object:  Table [dbo].[Apply_Indeed]    Script Date: 19-Dec-17 9:07:05 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ApplyIndeed](
	[ApplicationId] [int] IDENTITY(1,1) NOT NULL,
	[SuccessfulApply] [bit] NOT NULL,
	[Jobkey] [varchar](255) NOT NULL,
	[Url] [varchar](max) NULL,
	[Snippet] [varchar](255) NOT NULL,
	[JobTitle] [varchar](255) NOT NULL,
	[Company] [varchar](255) NOT NULL,
	[DateTimeApplied] [datetime] NOT NULL,
	[Sponsored] [varchar](255) NOT NULL,
	[Expired] [varchar](255) NOT NULL,
	[IndeedApply] [varchar](255) NOT NULL,
	[FormattedLocationFull] [varchar](255) NOT NULL,
	[FormattedRelativeTime] [varchar](255) NOT NULL,
	[OnMouseDown] [varchar](255) NOT NULL,
	[Latitude] [varchar](255) NOT NULL,
	[Longitude] [varchar](255) NOT NULL,
	[City] [varchar](255) NOT NULL,
	[State] [varchar](255) NOT NULL,
	[Country] [varchar](255) NOT NULL,
	[FormattedLocation] [varchar](255) NOT NULL,
	[Source] [varchar](255) NOT NULL,
	[Date] [varchar](255) NOT NULL,
	[SessionId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ApplicationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[IndeedAvailCountries]    Script Date: 19-Dec-17 9:07:55 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[IndeedAvailCountries](
	[AvailCountryId] [int] IDENTITY(1,1) NOT NULL,
	[Country] [varchar](250) NOT NULL,
	[CountryCode] [varchar](5) NOT NULL,
	[CheckLang] [bit] NOT NULL DEFAULT ((0)),
PRIMARY KEY CLUSTERED 
(
	[AvailCountryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[Users]    Script Date: 19-Dec-17 9:11:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[AppEmail] [varchar](255) NOT NULL,
	[AppName] [varchar](255) NOT NULL,
	[AppPhone] [varchar](100) NOT NULL,
	[AppResumePath] [varchar](255) NOT NULL,
	[AppSupportingDoc1] [varchar](255) NULL,
	[AppSupportingDoc2] [varchar](255) NULL,
	[AppSupportingDoc3] [varchar](255) NULL,
	[AppSupportingDoc4] [varchar](255) NULL,
	[AppSupportingDoc5] [varchar](255) NULL,
	[CoverLetter] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[SearchTerms]    Script Date: 19-Dec-17 9:12:06 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SearchTerms](
	[SearchTermId] [int] IDENTITY(1,1) NOT NULL,
	[Term] [varchar](250) NOT NULL,
	[UserId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SearchTermId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[Sessions]    Script Date: 19-Dec-17 9:12:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Sessions](
	[SessionId] [int] IDENTITY(1,1) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[UserId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((1)),
	[EndDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[SessionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[UsersCountriesToApply]    Script Date: 19-Dec-17 9:12:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[UsersCountriesToApply](
	[CountryApplyId] [int] IDENTITY(1,1) NOT NULL,
	[AvailCountryId] [int] NOT NULL,
	[LocationSearch] [varchar](250) NULL,
	[UserId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CountryApplyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

-- Add All Foreign Keys for Referential Integrity

ALTER TABLE [dbo].[SearchTerms]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO

ALTER TABLE [dbo].[Sessions]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO

ALTER TABLE [dbo].[UsersCountriesToApply]  WITH CHECK ADD FOREIGN KEY([AvailCountryId])
REFERENCES [dbo].[IndeedAvailCountries] ([AvailCountryId])
GO

ALTER TABLE [dbo].[UsersCountriesToApply]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO

ALTER TABLE [dbo].[ApplyIndeed]  WITH CHECK ADD FOREIGN KEY([SessionId])
REFERENCES [dbo].[Sessions] ([SessionId])
GO

-- INSERT DATA:

-- Indeed Avail Countries (in accordance with country codes available in Indeed Publisher API [fixed])

INSERT INTO IndeedAvailCountries VALUES
('Antarctica','aq',0),
('Argentina','ar',0),
('Australia','au',0),
('Austria','at',0),
('Bahrain','bh',0),
('Belgium','be',0),
('Brazil','br',0),
('Canada','ca',0),
('Chile','cl',0),
('China','cn',0),
('Colombia','co',0),
('Costa Rica','cr',0),
('Czech Republic','cz',0),
('Denmark','dk',0),
('Ecuador','ec',0),
('Egypt','eg',0),
('Finland','fi',0),
('France','fr',0),
('Germany','de',0),
('Greece','gr',0),
('Hong Kong','hk',0),
('Hungary','hu',0),
('India','in',0),
('Indonesia','id',0),
('Ireland','ie',0),
('Israel','il',0),
('Italy','it',0),
('Japan','jp',0),
('Kuwait','kw',0),
('Luxembourg','lu',0),
('Malaysia','my',0),
('Mexico','mx',0),
('Morocco','ma',0),
('Netherlands','nl',0),
('New Zealand','nz',0),
('Nigeria','ng',0),
('Norway','no',0),
('Oman','om',0),
('Pakistan','pk',0),
('Panama','pa',0),
('Peru','pe',0),
('Philippines','ph',0),
('Poland','pl',0),
('Portugal','pt',0),
('Qatar','qa',0),
('Romania','ro',0),
('Russia','ru',0),
('Saudi Arabia','sa',0),
('Singapore','sg',0),
('South Africa','za',0),
('South Korea','kr',0),
('Spain','es',0),
('Sweden','se',0),
('Switzerland','ch',0),
('Taiwan','tw',0),
('Thailand','th',0),
('Turkey','tr',0),
('Ukraine','ua',0),
('United Arab Emirates','ae',0),
('United Kingdom','gb',0),
('United States','us',0),
('Uruguay','uy',0),
('Venezuela','ve',0),
('Vietnam','vn',0)