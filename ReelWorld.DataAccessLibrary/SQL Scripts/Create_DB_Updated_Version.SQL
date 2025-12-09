USE [DMA-CSD-S241_10632087]
GO
/****** Object:  Table [dbo].[AccessLevel]    Script Date: 09-12-2025 09:16:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccessLevel](
	[AccessLevelId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[AccessLevelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[City]    Script Date: 09-12-2025 09:16:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[City](
	[CityID] [int] IDENTITY(1,1) NOT NULL,
	[City] [nvarchar](50) NOT NULL,
	[FK_Country_ID] [int] NOT NULL,
 CONSTRAINT [PK_City] PRIMARY KEY CLUSTERED 
(
	[CityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Country]    Script Date: 09-12-2025 09:16:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Country](
	[CountryID] [int] IDENTITY(1,1) NOT NULL,
	[Country] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED 
(
	[CountryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Event]    Script Date: 09-12-2025 09:16:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Event](
	[EventID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[Location] [nvarchar](200) NULL,
	[IsPublic] [bit] NOT NULL,
	[Limit] [int] NULL,
	[FK_Profile_ID] [int] NULL,
 CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED 
(
	[EventID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventProfile]    Script Date: 09-12-2025 09:16:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventProfile](
	[EventId] [int] NOT NULL,
	[ProfileId] [int] NOT NULL,
	[JoinedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[EventId] ASC,
	[ProfileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Interests]    Script Date: 09-12-2025 09:16:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Interests](
	[InterestsID] [int] IDENTITY(1,1) NOT NULL,
	[InterestName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Interests] PRIMARY KEY CLUSTERED 
(
	[InterestsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Profile]    Script Date: 09-12-2025 09:16:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Profile](
	[ProfileID] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[HashPassword] [nvarchar](255) NOT NULL,
	[Salt] [nvarchar](255) NOT NULL,
	[ProfileType] [nvarchar](50) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[MiddleName] [nvarchar](50) NULL,
	[Surname] [nvarchar](50) NOT NULL,
	[PhoneNo] [nvarchar](20) NULL,
	[Age] [int] NOT NULL,
	[Relationship] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[FK_City_ID] [int] NOT NULL,
	[StreetName] [nvarchar](100) NULL,
	[StreetNumber] [nvarchar](20) NULL,
	[ZipCode] [nvarchar](20) NULL,
	[FK_AccessLevel_Id] [int] NOT NULL,
 CONSTRAINT [PK_Profile] PRIMARY KEY CLUSTERED 
(
	[ProfileID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Profile_Email] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProfileInterests]    Script Date: 09-12-2025 09:16:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProfileInterests](
	[InterestsID] [int] NOT NULL,
	[ProfileID] [int] NOT NULL,
 CONSTRAINT [PK_UserInterests] PRIMARY KEY CLUSTERED 
(
	[ProfileID] ASC,
	[InterestsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EventProfile] ADD  DEFAULT (getdate()) FOR [JoinedAt]
GO
ALTER TABLE [dbo].[Profile] ADD  DEFAULT ((0)) FOR [Age]
GO
ALTER TABLE [dbo].[Profile] ADD  DEFAULT ((1)) FOR [FK_City_ID]
GO
ALTER TABLE [dbo].[Profile] ADD  DEFAULT ((2)) FOR [FK_AccessLevel_Id]
GO
ALTER TABLE [dbo].[City]  WITH CHECK ADD  CONSTRAINT [FK_City_Country] FOREIGN KEY([FK_Country_ID])
REFERENCES [dbo].[Country] ([CountryID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[City] CHECK CONSTRAINT [FK_City_Country]
GO
ALTER TABLE [dbo].[Event]  WITH CHECK ADD  CONSTRAINT [FK_Event_Profile] FOREIGN KEY([FK_Profile_ID])
REFERENCES [dbo].[Profile] ([ProfileID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Event] CHECK CONSTRAINT [FK_Event_Profile]
GO
ALTER TABLE [dbo].[EventProfile]  WITH CHECK ADD FOREIGN KEY([ProfileId])
REFERENCES [dbo].[Profile] ([ProfileID])
GO
ALTER TABLE [dbo].[EventProfile]  WITH CHECK ADD  CONSTRAINT [FK_EventProfile_Event] FOREIGN KEY([EventId])
REFERENCES [dbo].[Event] ([EventID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EventProfile] CHECK CONSTRAINT [FK_EventProfile_Event]
GO
ALTER TABLE [dbo].[Profile]  WITH CHECK ADD  CONSTRAINT [FK_Profile_AccessLevel] FOREIGN KEY([FK_AccessLevel_Id])
REFERENCES [dbo].[AccessLevel] ([AccessLevelId])
GO
ALTER TABLE [dbo].[Profile] CHECK CONSTRAINT [FK_Profile_AccessLevel]
GO
ALTER TABLE [dbo].[ProfileInterests]  WITH CHECK ADD  CONSTRAINT [FK_UserInterests_Interests] FOREIGN KEY([InterestsID])
REFERENCES [dbo].[Interests] ([InterestsID])
GO
ALTER TABLE [dbo].[ProfileInterests] CHECK CONSTRAINT [FK_UserInterests_Interests]
GO
ALTER TABLE [dbo].[ProfileInterests]  WITH CHECK ADD  CONSTRAINT [FK_UserInterests_Profile] FOREIGN KEY([ProfileID])
REFERENCES [dbo].[Profile] ([ProfileID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProfileInterests] CHECK CONSTRAINT [FK_UserInterests_Profile]
GO
