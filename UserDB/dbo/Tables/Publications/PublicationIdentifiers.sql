CREATE TABLE [dbo].[PublicationIdentifiers]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [PublicationId] BIGINT NOT NULL, 
    [IdentifierValue] VARCHAR(100) NULL, 
    [IdentifierName] VARCHAR(50) NULL, 
    [ISForm] VARCHAR(20) NULL, 
    [OriginSourceType] SMALLINT NOT NULL, 
    [VersionDate] DATETIME NOT NULL, 
    [CreatedAt] DATETIME NOT NULL, 
    CONSTRAINT [FK_PublicationIdentifiers_ToPublicationTable] FOREIGN KEY ([PublicationId]) REFERENCES [Publications]([Id])
)
