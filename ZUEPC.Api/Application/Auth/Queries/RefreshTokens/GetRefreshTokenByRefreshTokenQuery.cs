using MediatR;

namespace ZUEPC.Api.Application.Auth.Queries.RefreshTokens;

public class GetRefreshTokenByRefreshTokenQuery : IRequest<GetRefreshTokenByRefreshTokenQueryResponse>
{
	public string? RefreshToken { get; set; }
}
