CREATE TABLE [dbo].[Publications]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [PublishYear] INT NULL, 
    [DocumentType] VARCHAR(50) NULL,
    [OriginSourceType] SMALLINT NOT NULL, 
    [VersionDate] DATETIME NOT NULL, 
    [CreatedAt] DATETIME NOT NULL
)
