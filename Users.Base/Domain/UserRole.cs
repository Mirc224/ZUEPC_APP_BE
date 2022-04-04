using ZUEPC.Base.Enums.Users;
using ZUEPC.EvidencePublication.Domain.Common;

namespace ZUEPC.Users.Base.Domain;

public class UserRole : DomainBase
{
	public long UserId { get; set; }
	public RoleType RoleId { get; set; }
}
