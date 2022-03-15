CREATE TABLE [dbo].[PublicationActivities]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [PublicationId] BIGINT NOT NULL, 
    [ActivityYear] INT NULL, 
    [Category] VARCHAR(100) NULL, 
    [GovernmentGrant] VARCHAR(50) NULL, 
    [OriginSourceType] SMALLINT NOT NULL, 
    [VersionDate] DATETIME NOT NULL, 
    [CreatedAt] DATETIME NOT NULL, 
    CONSTRAINT [FK_PublicationActivities_ToPublicationTable] FOREIGN KEY ([PublicationId]) REFERENCES [Publications]([Id])
)
