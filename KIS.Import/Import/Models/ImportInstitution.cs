﻿namespace ZUEPC.Import.Import.Models;

public class ImportInstitution
{
	public int? Level { get; set; }
	public List<ImportInstitutionName> InstitutionNames { get; set; } = new();
	public List<ImportInstitutionExternDbId> InstitutionExternDbIds { get; set; } = new();

	public string? InstititutionType { get; set; }
	public string? InstititutionTag { get; set; }

	public class ImportInstitutionName
	{
		public string? NameType { get; set; }
		public string? Name { get; set; }
	}

	public class ImportInstitutionExternDbId
	{
		public string? InstitutionExternDbId { get; set; }
	}
}