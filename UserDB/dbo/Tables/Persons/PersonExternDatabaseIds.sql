CREATE TABLE [dbo].[PersonExternDatabaseIds]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [PersonId] BIGINT NOT NULL, 
    [ExternIdentifierValue] VARCHAR(50) NOT NULL,
    [OriginSourceType] SMALLINT NOT NULL, 
    [VersionDate] DATETIME NOT NULL, 
    [CreatedAt] DATETIME NOT NULL, 
    CONSTRAINT [FK_PersonExternDatabaseIds_ToPersonsTable] FOREIGN KEY ([PersonId]) REFERENCES [Persons]([Id])
)
