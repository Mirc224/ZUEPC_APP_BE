CREATE TABLE [dbo].[UserRoles]
(
	[UserId] BIGINT NOT NULL , 
    [RoleId] SMALLINT NOT NULL, 
    [CreatedAt] DATETIME NOT NULL DEFAULT SYSUTCDATETIME(), 
    CONSTRAINT PK_UserRoles PRIMARY KEY ([UserId],[RoleId]),
    CONSTRAINT FK_UserRoles_Users  FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] (Id),
    CONSTRAINT FK_UserRoles_Roles  FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles] (Id)
)
