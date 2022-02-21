using ZUEPC.Import.Import.Service;

namespace ZUEPC.Import.Import.Models;

public class ImportRelatedPublication
{
	public string? RelationType { get; set; }
	public string? CitationCategory { get; set; }
	public ImportPublication? RelatedPublication { get; set; }
}
