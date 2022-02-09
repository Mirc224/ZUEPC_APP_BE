--if not exists ( select 1 from dbo.[User])
--begin
--	insert into dbo.[User] (FirstName, LastName)
--	values ('Miroslav', 'Potocar'),
--	('Sue', 'Storm'),
--	('John', 'Smith'),
--	('Mary', 'Jones');
--end

if not exists ( select 1 from dbo.[Roles])
begin
	insert into dbo.[Roles] (Id, [Name])
	values (1, 'Admin'),
	(2, 'Editor'),
	(3, 'User');
end