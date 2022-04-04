using ZUEPC.Base.Enums.Users;
using ZUEPC.Base.ItemInterfaces;
using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.Users;

public class RoleModel : 
	ModelBase,
	IItemWithID<RoleType>
{
	public RoleType Id { get; set; }
	public string? Name { get; set; }
}
