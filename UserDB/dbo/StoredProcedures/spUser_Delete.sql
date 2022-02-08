CREATE PROCEDURE [dbo].[spUser_Delete]
	@Id int
AS
begin
	DELETE
	FROM dbo.[User]
	WHERE Id = @Id;
end
