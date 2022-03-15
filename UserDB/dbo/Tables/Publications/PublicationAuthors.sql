CREATE TABLE [dbo].[PublicationAuthors]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [PublicationId] BIGINT NOT NULL, 
    [PersonId] BIGINT NOT NULL, 
    [InstitutionId] BIGINT NOT NULL, 
    [ContributionRatio] FLOAT NULL, 
    [Role] VARCHAR(50) NULL, 
    [OriginSourceType] SMALLINT NOT NULL, 
    [VersionDate] DATETIME NOT NULL, 
    [CreatedAt] DATETIME NOT NULL, 
    CONSTRAINT [FK_PublicationAuthorPublicationId_ToPublicationTable] FOREIGN KEY ([PublicationId]) REFERENCES [Publications]([Id]),
    CONSTRAINT [FK_PublicationAuthorPersonId_ToPersonTable] FOREIGN KEY ([PersonId]) REFERENCES [Persons]([Id]),
    CONSTRAINT [FK_PublicationAuthorInstitutionId_ToInstitutionTable] FOREIGN KEY ([InstitutionId]) REFERENCES [Institutions]([Id])
)
