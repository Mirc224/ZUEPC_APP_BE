﻿CREATE TABLE [dbo].[UserRoles]
(
    [Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
	[UserId] BIGINT NOT NULL , 
    [RoleId] BIGINT NOT NULL, 
    [CreatedAt] DATETIME NOT NULL DEFAULT SYSUTCDATETIME(), 
    CONSTRAINT FK_UserRoles_Users  FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] (Id),
    CONSTRAINT FK_UserRoles_Roles  FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles] (Id)
)
