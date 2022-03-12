using MediatR;

namespace ZUEPC.Api.Application.Auth.Queries.RefreshTokens;

public class GetRefreshTokenByJwtIdQuery : IRequest<GetRefreshTokenByJwtIdQueryResponse>
{
	public string? JwtId { get; set; }
}
