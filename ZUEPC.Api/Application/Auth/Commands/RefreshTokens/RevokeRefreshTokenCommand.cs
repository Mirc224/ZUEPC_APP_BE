using MediatR;

namespace ZUEPC.Application.Auth.Commands.RefreshTokens;

public class RevokeRefreshTokenCommand : IRequest<RevokeRefreshTokenCommandResponse>
{
	public string? RefreshToken { get; set; }
}
