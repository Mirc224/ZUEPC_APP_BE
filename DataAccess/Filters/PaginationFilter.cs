namespace ZUEPC.DataAccess.Filters;

public class PaginationFilter
{
	public int PageNumber {
		get => _pageNumber; 
		set => _pageNumber = value < 1 ? 1 : value;
	}
	public int PageSize
	{
		get => _pageSize;
		set => _pageSize = value > 50 ? 50 : value;
	}
	private int _pageNumber = 1;
	private int _pageSize = 10;
}
