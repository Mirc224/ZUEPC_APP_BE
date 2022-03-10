namespace ZUEPC.Responses;

public class ResponseBase
{
	public bool Success { get; set; }
	public IEnumerable<string>? ErrorMessages { get; set; }
}
