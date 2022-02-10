namespace MVCAPIDemo.Application.Responses;

public class CQRSBaseResponse
{
	public bool Success { get; set; }
	public IEnumerable<string> ErrorMessages { get; set; }
}
