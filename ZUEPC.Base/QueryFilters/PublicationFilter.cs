namespace ZUEPC.Base.QueryFilters;

public class PublicationFilter : IQueryFilter
{
	public string[]? DocumentType { get; set; }
	public int[]? PublishYear { get; set; }
	public string[]? Name { get; set; }
	public string[]? NameType { get; set; }
	public string[]? IdentifierValue { get; set; }
	public string[]? IdentifierName { get; set; }
	public string[]? ISForm { get; set; }
	public string[]? ExternIdentifierValue { get; set; }
	public string[]? InstitutionName { get; set; }
	public string[]? AuthorName { get; set; }
	public int[]? ActivityYear { get; set; }
	public string[]? ActivityCategory { get; set; }
	public string[]? GovernmentGrant { get; set; }
}
