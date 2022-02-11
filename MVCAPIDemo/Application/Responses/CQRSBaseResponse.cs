using System.Text.Json.Serialization;

namespace MVCAPIDemo.Application.Responses;

public class CQRSBaseResponse
{
	[JsonIgnore]
	public bool Success { get; set; }

	public IEnumerable<string> ErrorMessages { get; set; }
}
