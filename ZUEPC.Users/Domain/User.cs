using ZUEPC.Base.ItemInterfaces;
using ZUEPC.Base.Domain;

namespace Users.Base.Domain;

public class User : 
	DomainBase,
	IItemWithID<long>
{
	public long Id { get; set; }
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
}
