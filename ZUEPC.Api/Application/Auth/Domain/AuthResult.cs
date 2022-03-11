using ZUEPC.Responses;

namespace ZUEPC.Auth.Domain;

public class AuthResult: ResponseBase
{
	public string? Token { get; set; }
	public string? RefreshToken { get; set; }
}
