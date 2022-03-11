using ZUEPC.Common.Entities;
using ZUEPC.Users.Base.Domain;

namespace ZUEPC.Api.Application.Users.Entities.Details;

public class UserDetails
{
	public long Id { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? Email { get; set; }
	public DateTime CreatedAt { get; set; }
	public IEnumerable<UserRole>? UserRoles { get; set; }

}
