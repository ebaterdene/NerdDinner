
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 10/07/2013 16:23:34
-- Generated from EDMX file: c:\Src\NerdDinner\NerdDinner\Models\NerdDinner.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [aspnet-NerdDinner-20131007133750];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_RSVP_To_Dinners]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RSVP] DROP CONSTRAINT [FK_RSVP_To_Dinners];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Dinners]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Dinners];
GO
IF OBJECT_ID(N'[dbo].[RSVP]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RSVP];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Suppers'
CREATE TABLE [dbo].[Suppers] (
    [DinnerID] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(50)  NOT NULL,
    [EventDate] datetime  NOT NULL,
    [Description] nvarchar(256)  NOT NULL,
    [HostedBy] nvarchar(20)  NOT NULL,
    [ContactPhone] nvarchar(20)  NOT NULL,
    [Address] nvarchar(20)  NOT NULL,
    [Country] nvarchar(30)  NOT NULL,
    [Latitude] float  NOT NULL,
    [Longtitude] float  NOT NULL
);
GO

-- Creating table 'RSVPs'
CREATE TABLE [dbo].[RSVPs] (
    [RsvpID] int IDENTITY(1,1) NOT NULL,
    [DinnerID] int  NOT NULL,
    [AttendeeName] nvarchar(30)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [DinnerID] in table 'Suppers'
ALTER TABLE [dbo].[Suppers]
ADD CONSTRAINT [PK_Suppers]
    PRIMARY KEY CLUSTERED ([DinnerID] ASC);
GO

-- Creating primary key on [RsvpID] in table 'RSVPs'
ALTER TABLE [dbo].[RSVPs]
ADD CONSTRAINT [PK_RSVPs]
    PRIMARY KEY CLUSTERED ([RsvpID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [DinnerID] in table 'RSVPs'
ALTER TABLE [dbo].[RSVPs]
ADD CONSTRAINT [FK_RSVP_To_Dinners]
    FOREIGN KEY ([DinnerID])
    REFERENCES [dbo].[Suppers]
        ([DinnerID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RSVP_To_Dinners'
CREATE INDEX [IX_FK_RSVP_To_Dinners]
ON [dbo].[RSVPs]
    ([DinnerID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------