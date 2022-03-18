namespace ZUEPC.DataAccess.Filters;

public class UserFilter 
	: IQueryFilter
{
	public string[]? Name { get; set; }
	public string[]? Email { get; set; }
	public int[]? UserRole { get; set; }
}
