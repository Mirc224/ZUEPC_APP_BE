CREATE TABLE [dbo].[Persons]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [BirthYear] INT NULL, 
    [DeathYear] INT NULL,
    [OriginSourceType] SMALLINT NOT NULL, 
    [VersionDate] DATETIME NOT NULL, 
    [CreatedAt] DATETIME NOT NULL
)
