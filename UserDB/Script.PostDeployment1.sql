if not exists(select 1 from [dbo].[Roles])
begin
	insert into dbo.[User] (Id, Name)
	values (1, 'Admin'),
	(2, 'Editor'),
	(3, 'User');
end