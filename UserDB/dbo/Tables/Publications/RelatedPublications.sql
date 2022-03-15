CREATE TABLE [dbo].[RelatedPublications]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [PublicationId] BIGINT NOT NULL, 
    [RelatedPublicationId] BIGINT NOT NULL, 
    [RelationType] VARCHAR(50) NULL, 
    [CitationCategory] VARCHAR(30) NULL, 
    [OriginSourceType] SMALLINT NOT NULL, 
    [VersionDate] DATETIME NOT NULL, 
    [CreatedAt] DATETIME NOT NULL, 
    CONSTRAINT [FK_PublicationRelatedPublicationPubId_ToPublicationTable] FOREIGN KEY ([PublicationId]) REFERENCES [Publications]([Id]),
    CONSTRAINT [FK_PublicationRelatedPublicationRelPubId_ToPublicationTable] FOREIGN KEY ([RelatedPublicationId]) REFERENCES [Publications]([Id])
)
