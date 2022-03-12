using MediatR;

namespace ZUEPC.Api.Application.Auth.Commands.RefreshTokens;

public class CreateRefreshTokenCommand :
	IRequest<CreateRefreshTokenCommandResponse>
{
	public long UserId { get; set; }
	public string? Token { get; set; }
	public string? JwtId { get; set; }
	public bool IsUsed { get; set; }
	public bool IsRevoked { get; set; }
	public DateTime ExpiryDate { get; set; }
}
