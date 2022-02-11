using System.Text.Json.Serialization;

namespace MVCAPIDemo.Application.Responses;

public class ResponseBase
{
	public bool Success { get; set; }

	public IEnumerable<string> ErrorMessages { get; set; }
}
