CREATE TABLE [dbo].[InstitutionExternDatabaseIds]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [InstitutionId] BIGINT NOT NULL, 
    [ExternIdentifierValue] NVARCHAR(100) NOT NULL,
    [OriginSourceType] SMALLINT NOT NULL, 
    [VersionDate] DATETIME NOT NULL, 
    [CreatedAt] DATETIME NOT NULL, 
    CONSTRAINT [FK_InstitutionExternDatabaseIds_ToInstitutionsTable] FOREIGN KEY ([InstitutionId]) REFERENCES [Institutions]([Id])
)
