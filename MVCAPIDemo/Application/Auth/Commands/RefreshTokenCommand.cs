using MediatR;

namespace MVCAPIDemo.Auth.Commands;

public class RefreshTokenCommand : IRequest<RefreshTokenCommandResponse>
{
	public string? Token { get; set; }	
	public string? RefreshToken { get; set; }
}
