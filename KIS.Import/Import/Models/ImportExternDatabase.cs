namespace ZUEPC.Import.Import.Models;

public class ImportExternDatabase
{
	public string? CREPCId { get; set; }
	public List<ImportExternDatabaseName> DatabaseNames { get; set; } = new();

	public class ImportExternDatabaseName
	{
		public string? Name { get; set; }
		public string? NameType { get; set; }
	}
}
