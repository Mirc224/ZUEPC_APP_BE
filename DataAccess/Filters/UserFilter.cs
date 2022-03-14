namespace ZUEPC.DataAccess.Filters;

public class UserFilter 
	: IQueryFilter
{
	public string[]? FirstName { get; set; }
	public string[]? LastName { get; set; }
	public string[]? Email { get; set; }
	public int[]? UserRole { get; set; }
}
