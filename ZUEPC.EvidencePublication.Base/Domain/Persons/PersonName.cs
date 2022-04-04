using ZUEPC.Base.ItemInterfaces;
using ZUEPC.EvidencePublication.Domain.Common;

namespace ZUEPC.EvidencePublication.Domain.Persons;

public class PersonName : 
	EPCDomainBase, 
	IPersonRelated
{
	public long PersonId { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? NameType { get; set; }
}
