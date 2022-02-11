using MVCAPIDemo.Application.Responses;

namespace MVCAPIDemo.Application.Domain;

public class AuthResult: ResponseBase
{
	public string Token { get; set; }
	public Guid RefreshToken { get; set; }
}
