CREATE PROCEDURE [dbo].[spUser_Delete]
	@Id int
AS
begin
	DELETE
	FROM dbo.[Users]
	WHERE Id = @Id;
end
