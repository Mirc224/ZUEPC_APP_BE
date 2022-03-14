using ZUEPC.DataAccess.Attributes.ModelAttributes;
using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.Users;

public class UserModel : ModelBase
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
	public string? Email { get; set; }
	public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }
}
