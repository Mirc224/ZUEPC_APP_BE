namespace DataAccess.Models;

public class RefreshTokenModel
{
	public int Id { get; set; }
	public int UserId { get; set; }
	public string Token { get; set; }
	public string JwtId { get; set; }
	public bool IsUsed { get; set; }
	public bool IsRevoked { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime ExpiryDate { get; set; }
}
