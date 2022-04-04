using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Users;
using ZUEPC.DataAccess.Models.Users;
using ZUEPC.Users.Domain;

namespace ZUEPC.Api.Application.Auth.Queries.RefreshTokens;

public class GetRefreshTokenByJwtIdQueryHandler :
	IRequestHandler<GetRefreshTokenByJwtIdQuery, GetRefreshTokenByJwtIdQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IRefreshTokenData _repository;

	public GetRefreshTokenByJwtIdQueryHandler(IMapper mapper, IRefreshTokenData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<GetRefreshTokenByJwtIdQueryResponse> Handle(GetRefreshTokenByJwtIdQuery request, CancellationToken cancellationToken)
	{
		if (request.JwtId is null)
		{
			return new() { Success = false };
		}
		RefreshTokenModel? queryResponse = await _repository.GetUserRefreshByJwtIdAsync(request.JwtId);
		if (queryResponse is null)
		{
			return new() { Success = false };
		}
		RefreshToken mappedResult = _mapper.Map<RefreshToken>(queryResponse);
		return new() { Success = true, Data = mappedResult };
	}
}
