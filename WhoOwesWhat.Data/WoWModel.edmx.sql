
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 09/12/2012 19:34:34
-- Generated from EDMX file: C:\tfs\WhoOwesWhat\WhoOwesWhat.Data\WoWModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [WhoOwesWhat];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_PersonCustomer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerSet] DROP CONSTRAINT [FK_PersonCustomer];
GO
IF OBJECT_ID(N'[dbo].[FK_PostPayer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerSet_Payer] DROP CONSTRAINT [FK_PostPayer];
GO
IF OBJECT_ID(N'[dbo].[FK_PostConsumer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerSet_Consumer] DROP CONSTRAINT [FK_PostConsumer];
GO
IF OBJECT_ID(N'[dbo].[FK_GroupPost]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PostSet] DROP CONSTRAINT [FK_GroupPost];
GO
IF OBJECT_ID(N'[dbo].[FK_Payer_inherits_Customer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerSet_Payer] DROP CONSTRAINT [FK_Payer_inherits_Customer];
GO
IF OBJECT_ID(N'[dbo].[FK_Consumer_inherits_Customer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerSet_Consumer] DROP CONSTRAINT [FK_Consumer_inherits_Customer];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[PersonSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PersonSet];
GO
IF OBJECT_ID(N'[dbo].[PostSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PostSet];
GO
IF OBJECT_ID(N'[dbo].[GroupSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GroupSet];
GO
IF OBJECT_ID(N'[dbo].[CustomerSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerSet];
GO
IF OBJECT_ID(N'[dbo].[Version4]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Version4];
GO
IF OBJECT_ID(N'[dbo].[CustomerSet_Payer]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerSet_Payer];
GO
IF OBJECT_ID(N'[dbo].[CustomerSet_Consumer]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerSet_Consumer];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'PersonSet'
CREATE TABLE [dbo].[PersonSet] (
    [PersonId] int IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(max)  NOT NULL,
    [PersonGuid] uniqueidentifier  NOT NULL,
    [PasswordHash] nvarchar(255)  NOT NULL,
    [PasswordSalt] nvarchar(255)  NOT NULL,
    [FullName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'PostSet'
CREATE TABLE [dbo].[PostSet] (
    [PostId] int IDENTITY(1,1) NOT NULL,
    [PostGuid] uniqueidentifier  NOT NULL,
    [Date] datetime  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [TotalCost] real  NOT NULL,
    [ISO4217CurrencyCode] nvarchar(max)  NOT NULL,
    [Version] int  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [Comment] nvarchar(max)  NULL,
    [GroupId] int  NULL
);
GO

-- Creating table 'GroupSet'
CREATE TABLE [dbo].[GroupSet] (
    [GroupId] int IDENTITY(1,1) NOT NULL,
    [GroupGuid] uniqueidentifier  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [IsDeleted] bit  NOT NULL
);
GO

-- Creating table 'CustomerSet'
CREATE TABLE [dbo].[CustomerSet] (
    [CustomerId] int IDENTITY(1,1) NOT NULL,
    [Amount] real  NOT NULL,
    [RelativeAmountInPercentage] int  NOT NULL,
    [AmountIsSetManually] bit  NOT NULL,
    [PersonId] int  NOT NULL
);
GO

-- Creating table 'Version4'
CREATE TABLE [dbo].[Version4] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'CustomerSet_Payer'
CREATE TABLE [dbo].[CustomerSet_Payer] (
    [PostId] int  NOT NULL,
    [CustomerId] int  NOT NULL
);
GO

-- Creating table 'CustomerSet_Consumer'
CREATE TABLE [dbo].[CustomerSet_Consumer] (
    [PostId] int  NOT NULL,
    [CustomerId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [PersonId] in table 'PersonSet'
ALTER TABLE [dbo].[PersonSet]
ADD CONSTRAINT [PK_PersonSet]
    PRIMARY KEY CLUSTERED ([PersonId] ASC);
GO

-- Creating primary key on [PostId] in table 'PostSet'
ALTER TABLE [dbo].[PostSet]
ADD CONSTRAINT [PK_PostSet]
    PRIMARY KEY CLUSTERED ([PostId] ASC);
GO

-- Creating primary key on [GroupId] in table 'GroupSet'
ALTER TABLE [dbo].[GroupSet]
ADD CONSTRAINT [PK_GroupSet]
    PRIMARY KEY CLUSTERED ([GroupId] ASC);
GO

-- Creating primary key on [CustomerId] in table 'CustomerSet'
ALTER TABLE [dbo].[CustomerSet]
ADD CONSTRAINT [PK_CustomerSet]
    PRIMARY KEY CLUSTERED ([CustomerId] ASC);
GO

-- Creating primary key on [Id] in table 'Version4'
ALTER TABLE [dbo].[Version4]
ADD CONSTRAINT [PK_Version4]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [CustomerId] in table 'CustomerSet_Payer'
ALTER TABLE [dbo].[CustomerSet_Payer]
ADD CONSTRAINT [PK_CustomerSet_Payer]
    PRIMARY KEY CLUSTERED ([CustomerId] ASC);
GO

-- Creating primary key on [CustomerId] in table 'CustomerSet_Consumer'
ALTER TABLE [dbo].[CustomerSet_Consumer]
ADD CONSTRAINT [PK_CustomerSet_Consumer]
    PRIMARY KEY CLUSTERED ([CustomerId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [PersonId] in table 'CustomerSet'
ALTER TABLE [dbo].[CustomerSet]
ADD CONSTRAINT [FK_PersonCustomer]
    FOREIGN KEY ([PersonId])
    REFERENCES [dbo].[PersonSet]
        ([PersonId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonCustomer'
CREATE INDEX [IX_FK_PersonCustomer]
ON [dbo].[CustomerSet]
    ([PersonId]);
GO

-- Creating foreign key on [PostId] in table 'CustomerSet_Payer'
ALTER TABLE [dbo].[CustomerSet_Payer]
ADD CONSTRAINT [FK_PostPayer]
    FOREIGN KEY ([PostId])
    REFERENCES [dbo].[PostSet]
        ([PostId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PostPayer'
CREATE INDEX [IX_FK_PostPayer]
ON [dbo].[CustomerSet_Payer]
    ([PostId]);
GO

-- Creating foreign key on [PostId] in table 'CustomerSet_Consumer'
ALTER TABLE [dbo].[CustomerSet_Consumer]
ADD CONSTRAINT [FK_PostConsumer]
    FOREIGN KEY ([PostId])
    REFERENCES [dbo].[PostSet]
        ([PostId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PostConsumer'
CREATE INDEX [IX_FK_PostConsumer]
ON [dbo].[CustomerSet_Consumer]
    ([PostId]);
GO

-- Creating foreign key on [GroupId] in table 'PostSet'
ALTER TABLE [dbo].[PostSet]
ADD CONSTRAINT [FK_GroupPost]
    FOREIGN KEY ([GroupId])
    REFERENCES [dbo].[GroupSet]
        ([GroupId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_GroupPost'
CREATE INDEX [IX_FK_GroupPost]
ON [dbo].[PostSet]
    ([GroupId]);
GO

-- Creating foreign key on [CustomerId] in table 'CustomerSet_Payer'
ALTER TABLE [dbo].[CustomerSet_Payer]
ADD CONSTRAINT [FK_Payer_inherits_Customer]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[CustomerSet]
        ([CustomerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [CustomerId] in table 'CustomerSet_Consumer'
ALTER TABLE [dbo].[CustomerSet_Consumer]
ADD CONSTRAINT [FK_Consumer_inherits_Customer]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[CustomerSet]
        ([CustomerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------