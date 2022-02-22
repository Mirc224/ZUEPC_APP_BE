using MediatR;

namespace ZUEPC.Application.Auth.Commands;

public class RevokeTokenCommand : IRequest<RevokeTokenCommandResponse>
{
	public string? RefreshToken { get; set; }
}
