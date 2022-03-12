using ZUEPC.EvidencePublication.Base.Domain.Common;

namespace ZUEPC.Users.Base.Domain;

public class RefreshToken : DomainBase
{
	public long UserId { get; set; }
	public string? Token { get; set; }
	public string? JwtId { get; set; }
	public bool IsUsed { get; set; }
	public bool IsRevoked { get; set; }
	public DateTime ExpiryDate { get; set; }
}
