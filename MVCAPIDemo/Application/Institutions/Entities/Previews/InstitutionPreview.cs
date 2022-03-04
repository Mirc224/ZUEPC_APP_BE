﻿using ZUEPC.EvidencePublication.Base.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Entities.Previews;

public class InstitutionPreview
{
	public long Id { get; set; }
	public int? Level { get; set; }
	public string? InstitutionType { get; set; }
	public ICollection<InstitutionName>? Names { get; set; }
	public ICollection<InstitutionExternDatabaseId>? ExternDatabaseIds { get; set; }
}
