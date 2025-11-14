USE [DMA-CSD-S241_10632087];
GO

---------------------------------------------------------
-- TABLE: Country
---------------------------------------------------------
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

CREATE TABLE [dbo].[Country](
    [CountryID] INT IDENTITY(1,1) NOT NULL,
    [Country] NVARCHAR(50) NOT NULL,
    CONSTRAINT PK_Country PRIMARY KEY CLUSTERED (CountryID ASC)
);
GO

---------------------------------------------------------
-- TABLE: City
---------------------------------------------------------
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

CREATE TABLE [dbo].[City](
    [CityID] INT IDENTITY(1,1) NOT NULL,
    [City] NVARCHAR(50) NOT NULL,
    [FK_Country_ID] INT NOT NULL,
    CONSTRAINT PK_City PRIMARY KEY CLUSTERED (CityID ASC)
);
GO

---------------------------------------------------------
-- TABLE: Interests
---------------------------------------------------------
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

CREATE TABLE [dbo].[Interests](
    [InterestsID] INT IDENTITY(1,1) NOT NULL,
    [InterestName] NVARCHAR(50) NOT NULL,
    CONSTRAINT PK_Interests PRIMARY KEY CLUSTERED (InterestsID ASC)
);
GO

---------------------------------------------------------
-- TABLE: Profile
---------------------------------------------------------
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

CREATE TABLE [dbo].[Profile](
    [ProfileID] INT IDENTITY(1,1) NOT NULL,
    [Email] NVARCHAR(50) NOT NULL,
    [HashPassword] NVARCHAR(255) NOT NULL,
    [Salt] NVARCHAR(255) NOT NULL,
    [ProfileType] NVARCHAR(50) NOT NULL,
    [FirstName] NVARCHAR(50) NOT NULL,
    [MiddleName] NVARCHAR(50) NULL,
    [Surname] NVARCHAR(50) NOT NULL,
    CONSTRAINT PK_Profile PRIMARY KEY CLUSTERED (ProfileID ASC),
    CONSTRAINT UQ_Profile_Email UNIQUE NONCLUSTERED (Email ASC)
);
GO

---------------------------------------------------------
-- TABLE: User
---------------------------------------------------------
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

CREATE TABLE [dbo].[User](
    [UserID] INT IDENTITY(1,1) NOT NULL,
    [Phoneno] NVARCHAR(20) NULL,
    [Age] INT NOT NULL,
    [RelationShip] NVARCHAR(50) NULL,
    [Description] NVARCHAR(MAX) NULL,
    [FK_Profile_ID] INT NOT NULL,
    [FK_City_ID] INT NOT NULL,
    CONSTRAINT PK_User PRIMARY KEY CLUSTERED (UserID ASC)
);
GO

---------------------------------------------------------
-- TABLE: UserInterests
---------------------------------------------------------
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

CREATE TABLE [dbo].[UserInterests](
    [UserID] INT NOT NULL,
    [InterestsID] INT NOT NULL,
    CONSTRAINT PK_UserInterests PRIMARY KEY CLUSTERED (UserID ASC, InterestsID ASC)
);
GO

---------------------------------------------------------
-- FOREIGN KEYS
---------------------------------------------------------

-- City → Country
ALTER TABLE [dbo].[City]
ADD CONSTRAINT FK_City_Country
FOREIGN KEY ([FK_Country_ID])
REFERENCES [dbo].[Country] ([CountryID])
ON UPDATE CASCADE
ON DELETE CASCADE;
GO

-- User → City
ALTER TABLE [dbo].[User]
ADD CONSTRAINT FK_User_City
FOREIGN KEY ([FK_City_ID])
REFERENCES [dbo].[City] ([CityID])
ON UPDATE CASCADE
ON DELETE CASCADE;
GO

-- User → Profile
ALTER TABLE [dbo].[User]
ADD CONSTRAINT FK_User_Profile
FOREIGN KEY ([FK_Profile_ID])
REFERENCES [dbo].[Profile] ([ProfileID]);
GO

-- UserInterests → Interests
ALTER TABLE [dbo].[UserInterests]
ADD CONSTRAINT FK_UserInterests_Interests
FOREIGN KEY ([InterestsID])
REFERENCES [dbo].[Interests] ([InterestsID]);
GO

-- UserInterests → User
ALTER TABLE [dbo].[UserInterests]
ADD CONSTRAINT FK_UserInterests_User
FOREIGN KEY ([UserID])
REFERENCES [dbo].[User] ([UserID]);
GO
