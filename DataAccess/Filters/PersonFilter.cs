namespace ZUEPC.DataAccess.Filters;

public class PersonFilter : IQueryFilter
{
	public int[]? BirthYear { get; set; }
	public int[]? DeathYear { get; set; }
	public string[]? FirstName { get; set; }
	public string[]? LastName { get; set; }
	public string[]? NameType { get; set; }
	public string[]? ExternIdentifierValue { get; set; }
}
