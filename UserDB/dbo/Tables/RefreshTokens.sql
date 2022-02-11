CREATE TABLE [dbo].[RefreshTokens]
(
    [Token] UNIQUEIDENTIFIER NOT NULL, 
    [UserId] INT NOT NULL, 
    [JwtId] VARCHAR(50) NOT NULL, 
    [IsUsed] BIT NOT NULL, 
    [IsRevoked] BIT NOT NULL, 
    [CreatedAt] DATE NOT NULL, 
    [ExpiryDate] DATE NOT NULL, 
    CONSTRAINT [FK_RefreshTokens_ToTable_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]), 
    CONSTRAINT [PK_RefreshTokens] PRIMARY KEY ([Token])
)
