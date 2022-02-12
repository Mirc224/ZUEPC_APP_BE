using MVCAPIDemo.Common.Responses;

namespace MVCAPIDemo.Auth.Domain;

public class AuthResult: ResponseBase
{
	public string? Token { get; set; }
	public Guid RefreshToken { get; set; }
}
