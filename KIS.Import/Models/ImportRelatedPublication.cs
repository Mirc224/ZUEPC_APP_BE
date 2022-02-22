namespace ZUEPC.Import.Models;

public class ImportRelatedPublication
{
	public string? RelationType { get; set; }
	public string? CitationCategory { get; set; }
	public ImportPublication? RelatedPublication { get; set; }
}
