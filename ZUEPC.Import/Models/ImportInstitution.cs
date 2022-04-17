using ZUEPC.Import.Models.Common;

namespace ZUEPC.Import.Models;

public class ImportInstitution
{
	public int? Level { get; set; }
	public string? InstititutionType { get; set; }
	public List<ImportInstitutionName> InstitutionNames { get; set; } = new();
	public List<ImportInstitutionExternDatabaseId> InstitutionExternDatabaseIds { get; set; } = new();

	public class ImportInstitutionName
	{
		public string? NameType { get; set; }
		public string Name { get; set; } = "";
	}

	public class ImportInstitutionExternDatabaseId : EPCImportExternDatabaseIdBase
	{
	}
}