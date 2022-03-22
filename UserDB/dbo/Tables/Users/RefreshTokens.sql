CREATE TABLE [dbo].[RefreshTokens]
(
    [Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [Token] VARCHAR(50) NOT NULL, 
    [UserId] BIGINT NOT NULL, 
    [JwtId] VARCHAR(50) NOT NULL, 
    [IsUsed] BIT NOT NULL, 
    [IsRevoked] BIT NOT NULL, 
    [CreatedAt] DATETIME NOT NULL DEFAULT SYSUTCDATETIME(), 
    [ExpiryDate] DATETIME NOT NULL, 
    CONSTRAINT [FK_RefreshTokens_ToTable_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]), 
)
