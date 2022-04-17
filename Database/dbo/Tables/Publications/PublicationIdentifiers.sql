CREATE TABLE [dbo].[PublicationIdentifiers]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [PublicationId] BIGINT NOT NULL, 
    [IdentifierValue] NVARCHAR(100) NULL, 
    [IdentifierName] NVARCHAR(100) NULL, 
    [ISForm] NVARCHAR(50) NULL, 
    [OriginSourceType] SMALLINT NOT NULL, 
    [VersionDate] DATETIME NOT NULL, 
    [CreatedAt] DATETIME NOT NULL, 
    CONSTRAINT [FK_PublicationIdentifiers_ToPublicationTable] FOREIGN KEY ([PublicationId]) REFERENCES [Publications]([Id])
)
