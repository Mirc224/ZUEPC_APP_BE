using DataAccess.Enums;

namespace MVCAPIDemo.Application.Domain;

public class User
{
	public int Id { get; set; } = 0;
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public List<RolesType> Roles { get; set; } = new List<RolesType>(0);
}
