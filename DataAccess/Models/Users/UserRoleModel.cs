using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.Users;

public class UserRoleModel : ModelBase
{
    public long UserId { get; set; }
    public long RoleId { get; set; }
}
