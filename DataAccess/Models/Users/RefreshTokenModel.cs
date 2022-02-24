namespace ZUEPC.DataAccess.Models.Users;

public class RefreshTokenModel
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public int Id { get; set; }
	public int UserId { get; set; }
	public Guid Token { get; set; }
	public string JwtId { get; set; }
	public bool IsUsed { get; set; }
	public bool IsRevoked { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime ExpiryDate { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
