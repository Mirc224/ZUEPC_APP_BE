CREATE TABLE [dbo].[Institutions]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [Level] SMALLINT NULL, 
    [InstitutionType] NVARCHAR(100) NULL,
    [OriginSourceType] SMALLINT NOT NULL, 
    [VersionDate] DATETIME NOT NULL, 
    [CreatedAt] DATETIME NOT NULL
)
