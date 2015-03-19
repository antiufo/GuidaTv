
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 01/30/2011 11:22:43
-- Generated from EDMX file: D:\Dati\My Dropbox\DocAnd\Visual Studio\Projects\TvGuide\MyMoviesDatabaseDump\Movies.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Movies];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_MovieActor_Movie]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MovieActor] DROP CONSTRAINT [FK_MovieActor_Movie];
GO
IF OBJECT_ID(N'[dbo].[FK_MovieActor_Actor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MovieActor] DROP CONSTRAINT [FK_MovieActor_Actor];
GO
IF OBJECT_ID(N'[dbo].[FK_MovieDirector]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Movies] DROP CONSTRAINT [FK_MovieDirector];
GO
IF OBJECT_ID(N'[dbo].[FK_CountryMovie_Country]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CountryMovie] DROP CONSTRAINT [FK_CountryMovie_Country];
GO
IF OBJECT_ID(N'[dbo].[FK_CountryMovie_Movie]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CountryMovie] DROP CONSTRAINT [FK_CountryMovie_Movie];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Movies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Movies];
GO
IF OBJECT_ID(N'[dbo].[Actors]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Actors];
GO
IF OBJECT_ID(N'[dbo].[Directors]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Directors];
GO
IF OBJECT_ID(N'[dbo].[Countries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Countries];
GO
IF OBJECT_ID(N'[dbo].[MovieActor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MovieActor];
GO
IF OBJECT_ID(N'[dbo].[CountryMovie]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CountryMovie];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Movies'
CREATE TABLE [dbo].[Movies] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Genre] nvarchar(max)  NULL,
    [Year] int  NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Summary] nvarchar(max)  NULL,
    [ShortDescription] nvarchar(max)  NULL,
    [Rating] real  NULL,
    [Suggestion] smallint  NULL,
    [ShortName] nvarchar(max)  NULL,
    [TrailerCode] nvarchar(max)  NULL,
    [ImageCode] nvarchar(max)  NULL,
    [MyMoviesId] int  NOT NULL,
    [Director_Id] int  NULL
);
GO

-- Creating table 'Actors'
CREATE TABLE [dbo].[Actors] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [MyMoviesId] int  NOT NULL
);
GO

-- Creating table 'Directors'
CREATE TABLE [dbo].[Directors] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [MyMoviesId] int  NOT NULL
);
GO

-- Creating table 'Countries'
CREATE TABLE [dbo].[Countries] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'MovieActor'
CREATE TABLE [dbo].[MovieActor] (
    [Movies_Id] int  NOT NULL,
    [Actors_Id] int  NOT NULL
);
GO

-- Creating table 'CountryMovie'
CREATE TABLE [dbo].[CountryMovie] (
    [Countries_Id] int  NOT NULL,
    [Movies_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Movies'
ALTER TABLE [dbo].[Movies]
ADD CONSTRAINT [PK_Movies]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Actors'
ALTER TABLE [dbo].[Actors]
ADD CONSTRAINT [PK_Actors]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Directors'
ALTER TABLE [dbo].[Directors]
ADD CONSTRAINT [PK_Directors]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Countries'
ALTER TABLE [dbo].[Countries]
ADD CONSTRAINT [PK_Countries]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Movies_Id], [Actors_Id] in table 'MovieActor'
ALTER TABLE [dbo].[MovieActor]
ADD CONSTRAINT [PK_MovieActor]
    PRIMARY KEY NONCLUSTERED ([Movies_Id], [Actors_Id] ASC);
GO

-- Creating primary key on [Countries_Id], [Movies_Id] in table 'CountryMovie'
ALTER TABLE [dbo].[CountryMovie]
ADD CONSTRAINT [PK_CountryMovie]
    PRIMARY KEY NONCLUSTERED ([Countries_Id], [Movies_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Movies_Id] in table 'MovieActor'
ALTER TABLE [dbo].[MovieActor]
ADD CONSTRAINT [FK_MovieActor_Movie]
    FOREIGN KEY ([Movies_Id])
    REFERENCES [dbo].[Movies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Actors_Id] in table 'MovieActor'
ALTER TABLE [dbo].[MovieActor]
ADD CONSTRAINT [FK_MovieActor_Actor]
    FOREIGN KEY ([Actors_Id])
    REFERENCES [dbo].[Actors]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MovieActor_Actor'
CREATE INDEX [IX_FK_MovieActor_Actor]
ON [dbo].[MovieActor]
    ([Actors_Id]);
GO

-- Creating foreign key on [Director_Id] in table 'Movies'
ALTER TABLE [dbo].[Movies]
ADD CONSTRAINT [FK_MovieDirector]
    FOREIGN KEY ([Director_Id])
    REFERENCES [dbo].[Directors]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MovieDirector'
CREATE INDEX [IX_FK_MovieDirector]
ON [dbo].[Movies]
    ([Director_Id]);
GO

-- Creating foreign key on [Countries_Id] in table 'CountryMovie'
ALTER TABLE [dbo].[CountryMovie]
ADD CONSTRAINT [FK_CountryMovie_Country]
    FOREIGN KEY ([Countries_Id])
    REFERENCES [dbo].[Countries]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Movies_Id] in table 'CountryMovie'
ALTER TABLE [dbo].[CountryMovie]
ADD CONSTRAINT [FK_CountryMovie_Movie]
    FOREIGN KEY ([Movies_Id])
    REFERENCES [dbo].[Movies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CountryMovie_Movie'
CREATE INDEX [IX_FK_CountryMovie_Movie]
ON [dbo].[CountryMovie]
    ([Movies_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------