using ZUEPC.DataAccess.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Users.Base.Domain;

public class User
{
	public int Id { get; set; } = 0;
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	[JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
	public List<RoleType> Roles { get; set; } = new List<RoleType>(0);
}
