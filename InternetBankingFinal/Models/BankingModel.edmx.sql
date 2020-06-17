
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/17/2020 00:42:05
-- Generated from EDMX file: C:\Users\ariel\Documents\InternetBanking\INTERNETBANKINGFINAL\InternetBankingFinal\Models\BankingModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [IBankingDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Log4NetLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Log4NetLog];
GO
IF OBJECT_ID(N'[IBankingDBModelStoreContainer].[User]', 'U') IS NOT NULL
    DROP TABLE [IBankingDBModelStoreContainer].[User];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Log4NetLog'
CREATE TABLE [dbo].[Log4NetLog] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NOT NULL,
    [Thread] varchar(255)  NOT NULL,
    [Level] varchar(50)  NOT NULL,
    [Logger] varchar(255)  NOT NULL,
    [Message] varchar(4000)  NOT NULL,
    [Exception] varchar(2000)  NULL
);
GO

-- Creating table 'User'
CREATE TABLE [dbo].[User] (
    [UserId] int IDENTITY(1,1) NOT NULL,
    [Nombre] varchar(150)  NOT NULL,
    [Apellido] varchar(150)  NOT NULL,
    [DateBirth] datetime  NULL,
    [Correo] varchar(200)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [IsEmailVerified] bit  NOT NULL,
    [ActivationCode] uniqueidentifier  NOT NULL,
    [Cedula] nvarchar(50)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Log4NetLog'
ALTER TABLE [dbo].[Log4NetLog]
ADD CONSTRAINT [PK_Log4NetLog]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [UserId] in table 'User'
ALTER TABLE [dbo].[User]
ADD CONSTRAINT [PK_User]
    PRIMARY KEY CLUSTERED ([UserId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------