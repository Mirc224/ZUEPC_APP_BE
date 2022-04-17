using ZUEPC.Base.Enums.Users;

namespace Users.Base.Application.Domain;

public class Role
{
    public RoleType Id { get; set; }
    public string? Name { get; set; }
}
