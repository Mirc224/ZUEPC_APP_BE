using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.Users;

public class RefreshTokenModel : ModelBase
{
	public long UserId { get; set; }
	public string? Token { get; set; }
	public string? JwtId { get; set; }
	public bool IsUsed { get; set; }
	public bool IsRevoked { get; set; }
	public DateTime ExpiryDate { get; set; }
}
