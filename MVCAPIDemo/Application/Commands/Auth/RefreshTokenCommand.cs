using MediatR;
using MVCAPIDemo.Application.Commands.Auth;

namespace MVCAPIDemo.Application.Commands.Auth;

public class RefreshTokenCommand : IRequest<RefreshTokenCommandResponse>
{
	public string Token { get; set; }	
	public string RefreshToken { get; set; }
}
