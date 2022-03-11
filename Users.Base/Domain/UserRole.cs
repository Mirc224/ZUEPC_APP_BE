using ZUEPC.EvidencePublication.Base.Domain.Common;

namespace ZUEPC.Users.Base.Domain;

public class UserRole : DomainBase
{
	public long UserId { get; set; }
	public long RoleId { get; set; }
}
