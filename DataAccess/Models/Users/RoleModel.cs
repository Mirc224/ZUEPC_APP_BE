using ZUEPC.DataAccess.Enums;

namespace ZUEPC.DataAccess.Models.Users;

public class RoleModel
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public RoleType Id { get; set; }
	public string Name { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
