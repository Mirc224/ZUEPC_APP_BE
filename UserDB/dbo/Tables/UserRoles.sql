CREATE TABLE [dbo].[UserRoles]
(
	[UserId] INT NOT NULL , 
    [RoleId] SMALLINT NOT NULL, 
    PRIMARY KEY ([RoleId], [UserId]),
    CONSTRAINT FK_UserRoles_User  FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] (Id),
    CONSTRAINT FK_UserRoles_Roles  FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles] (Id)
)
