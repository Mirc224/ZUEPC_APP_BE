namespace ZUEPC.Base.QueryFilters;

public class PersonFilter : IQueryFilter
{
	public int[]? BirthYear { get; set; }
	public int[]? DeathYear { get; set; }
	public string[]? Name { get; set; }
	public string[]? NameType { get; set; }
	public string[]? ExternIdentifierValue { get; set; }
}
