namespace ZUEPC.Import.Import.Models;

public class ImportPerson
{
	public List<ImportPersonName> PersonNames { get; set; } = new();
	public List<ImportPersonExternDbId> PersonExternDbIds { get; set; } = new();
	public int? BirthYear { get; set; }
	public int? DeathYear { get; set; }

	public class ImportPersonName
	{
		public string? NameType { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
	}

	public class ImportPersonExternDbId
	{
		public string? PersonExternDbId { get; set; }
	}
}
