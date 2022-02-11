namespace MVCAPIDemo.Application.Domain;

public class AuthResult
{
	public string Token { get; set; }
	public string RefreshToken { get; set; }
	public bool Success { get; set; }
	public List<string> ErrorMessages { get; set; }
}
