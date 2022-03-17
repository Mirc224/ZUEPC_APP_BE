CREATE TABLE [dbo].[InstitutionNames]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [InstitutionId] BIGINT NOT NULL, 
    [NameType] VARCHAR(50) NULL, 
    [Name] VARCHAR(250) NULL, 
    [OriginSourceType] SMALLINT NOT NULL, 
    [VersionDate] DATETIME NOT NULL, 
    [CreatedAt] DATETIME NOT NULL, 
    CONSTRAINT [FK_InstitutionNames_ToInstitituionsTable] FOREIGN KEY ([InstitutionId]) REFERENCES [Institutions]([Id])
)
