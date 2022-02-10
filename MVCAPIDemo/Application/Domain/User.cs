using DataAccess.Enums;

namespace MVCAPIDemo.Application.Domain;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
	public List<RolesType> Roles { get; set; }
}
