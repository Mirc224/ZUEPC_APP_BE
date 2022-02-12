using DataAccess.Enums;

namespace Users.Base.Application.Domain;

public class Role
{
    public RolesType Id { get; set; }
    public string? Name { get; set; }
}
