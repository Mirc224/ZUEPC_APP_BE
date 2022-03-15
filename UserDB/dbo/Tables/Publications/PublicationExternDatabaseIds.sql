CREATE TABLE [dbo].[PublicationExternDatabaseIds]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [PublicationId] BIGINT NOT NULL, 
    [ExternIdentifierValue] VARCHAR(100) NOT NULL,
    [OriginSourceType] SMALLINT NOT NULL, 
    [VersionDate] DATETIME NOT NULL, 
    [CreatedAt] DATETIME NOT NULL, 
    CONSTRAINT [FK_PublicationExternDatabaseIds_ToPublicationTable] FOREIGN KEY ([PublicationId]) REFERENCES [Publications]([Id])
)
