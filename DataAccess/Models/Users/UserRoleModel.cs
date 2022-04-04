using ZUEPC.Base.Enums.Users;
using ZUEPC.Base.ItemInterfaces;
using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.Users;

public class UserRoleModel : 
	ModelBase
{
    public long UserId { get; set; }
    public RoleType RoleId { get; set; }
}
