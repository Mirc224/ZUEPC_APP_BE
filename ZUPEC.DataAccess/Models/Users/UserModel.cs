using ZUEPC.DataAccess.Attributes.ModelAttributes;
using ZUEPC.Base.ItemInterfaces;
using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.Users;

public class UserModel :
	ModelBase,
	IItemWithID<long>
{
	[ExcludeFromInsert]
	[ExcludeFromUpdate]
	public long Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
	public string? Email { get; set; }
	public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }
}
