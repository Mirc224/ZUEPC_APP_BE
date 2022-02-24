namespace ZUEPC.DataAccess.Models.Users;

public class UserModel
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
	public string Email { get; set; }
	public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
