using ZUEPC.EvidencePublication.Base.Domain.Common;

namespace Users.Base.Domain;

public class User : DomainBase
{
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
}
