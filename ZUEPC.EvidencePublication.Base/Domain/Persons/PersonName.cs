﻿using ZUEPC.EvidencePublication.Base.Domain.Common;
using ZUEPC.EvidencePublication.Base.Domain.Common.Interfaces;

namespace ZUEPC.EvidencePublication.Base.Domain.Persons;

public class PersonName : EPCDomainBase, IPersonRelated
{
	public long PersonId { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? NameType { get; set; }
}
