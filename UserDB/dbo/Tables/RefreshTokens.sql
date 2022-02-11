CREATE TABLE [dbo].[RefreshTokens]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NOT NULL, 
    [Token] VARCHAR(MAX) NOT NULL, 
    [JwtId] VARCHAR(50) NOT NULL, 
    [IsUsed] BIT NOT NULL, 
    [IsRevoked] BIT NOT NULL, 
    [CreatedAt] DATE NOT NULL, 
    [ExpiryDate] DATE NOT NULL, 
    CONSTRAINT [FK_RefreshTokens_ToTable_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id])
)
