using ZUEPC.Base.Enums.Users;
using ZUEPC.Base.Domain;

namespace ZUEPC.Users.Domain;

public class UserRole : DomainBase
{
	public long UserId { get; set; }
	public RoleType RoleId { get; set; }
}
