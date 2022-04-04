CREATE TABLE [dbo].[PersonNames]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [PersonId] BIGINT NOT NULL, 
    [FirstName] NVARCHAR(100) NULL, 
    [LastName] NVARCHAR(100) NULL, 
    [NameType] NVARCHAR(100) NULL, 
    [OriginSourceType] SMALLINT NOT NULL, 
    [VersionDate] DATETIME NOT NULL, 
    [CreatedAt] DATETIME NOT NULL, 
    CONSTRAINT [FK_PersonNames_ToPersonsTable] FOREIGN KEY ([PersonId]) REFERENCES [Persons]([Id])
)
