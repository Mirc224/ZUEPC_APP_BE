CREATE PROCEDURE [dbo].[spUser_Insert]
	@FirstName nvarchar(50),
	@LastName nvarchar(50)
AS
begin
	insert into dbo.[Users] (FirstName, LastName)
	values (@FirstName, @LastName);
end
