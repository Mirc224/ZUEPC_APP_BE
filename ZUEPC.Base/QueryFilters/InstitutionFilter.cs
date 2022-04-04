namespace ZUEPC.Base.QueryFilters;

public class InstitutionFilter : IQueryFilter
{
	public int[]? Level { get; set; }
	public string[]? InstitutionType { get; set; }
	public string[]? Name { get; set; }
	public string[]? NameType { get; set; }
	public string[]? ExternIdentifierValue { get; set; }
}
