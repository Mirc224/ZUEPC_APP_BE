using MediatR;

namespace MVCAPIDemo.Application.Commands.Auth;

public class RevokeTokenCommand : IRequest<RevokeTokenCommandResponse>
{
	public string RefreshToken { get; set; }
}
