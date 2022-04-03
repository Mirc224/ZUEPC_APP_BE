using ZUEPC.DataAccess.Interfaces;
using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.Users;

public class RoleModel : 
	ModelBase,
	IItemWithID<long>
{
	public long Id { get; set; }
	public string? Name { get; set; }
}
