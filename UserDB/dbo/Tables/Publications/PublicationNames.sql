CREATE TABLE [dbo].[PublicationNames]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [PublicationId] BIGINT NOT NULL, 
    [Name] VARCHAR(1000) NULL, 
    [NameType] VARCHAR(50) NULL, 
    [OriginSourceType] SMALLINT NOT NULL, 
    [VersionDate] DATETIME NOT NULL, 
    [CreatedAt] DATETIME NOT NULL, 
    CONSTRAINT [FK_PublicationNames_ToPublicationTable] FOREIGN KEY ([PublicationId]) REFERENCES [Publications]([Id])
)
