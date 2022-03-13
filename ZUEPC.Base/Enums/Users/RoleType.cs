using System.Runtime.Serialization;

namespace ZUEPC.Base.Enums.Users;


public enum RoleType : long
{
	[EnumMember(Value = "ADMIN")]
	ADMIN = 1,
	[EnumMember(Value = "EDITOR")]
	EDITOR = 2,
	[EnumMember(Value = "USER")]
	USER = 3
}
