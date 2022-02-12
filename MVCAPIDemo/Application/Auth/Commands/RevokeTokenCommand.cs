using MediatR;

namespace MVCAPIDemo.Auth.Commands;

public class RevokeTokenCommand : IRequest<RevokeTokenCommandResponse>
{
	public string? RefreshToken { get; set; }
}
