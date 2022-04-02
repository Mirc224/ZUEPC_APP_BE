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
	public string OrderBy 
	{ 
		get => _orderBy; 
		set
		{
			if(string.IsNullOrWhiteSpace(value))
			{
				_orderBy = "CreatedAt";
				return;
			}
			_orderBy = string.Concat(value[0].ToString().ToUpper(), value.AsSpan(1));
		}
	}
	public string Order 
	{
		get => _order;
		set
		{
			if (string.IsNullOrWhiteSpace(value) || 
				(value.ToLower() != "asc" && 
				value.ToLower() != "desc"))
			{
				return;
			}
			_order = value;
		}
	}

	private string _orderBy = "CreatedAt";
	private string _order = "DESC";

	private int _pageNumber = 1;
	private int _pageSize = 10;
}
