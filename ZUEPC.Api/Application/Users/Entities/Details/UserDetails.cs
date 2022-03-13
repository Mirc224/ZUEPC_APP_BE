using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ZUEPC.Base.Enums.Users;

namespace ZUEPC.Api.Application.Users.Entities.Details;

public class UserDetails
{
	public long Id { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? Email { get; set; }
	public DateTime CreatedAt { get; set; }
	[JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
	public IEnumerable<RoleType>? UserRoles { get; set; }

}
