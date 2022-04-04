using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Users;
using ZUEPC.DataAccess.Models.Users;
using ZUEPC.Users.Domain;

namespace ZUEPC.Api.Application.Auth.Queries.RefreshTokens;

public class GetRefreshTokenByRefreshTokenQueryHandler
	: IRequestHandler<GetRefreshTokenByRefreshTokenQuery, GetRefreshTokenByRefreshTokenQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IRefreshTokenData _repository;

	public GetRefreshTokenByRefreshTokenQueryHandler(IMapper mapper, IRefreshTokenData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<GetRefreshTokenByRefreshTokenQueryResponse> Handle(GetRefreshTokenByRefreshTokenQuery request, CancellationToken cancellationToken)
	{
		if (request.RefreshToken is null)
		{
			return new() { Success = false };
		}
		RefreshTokenModel? queryResult = await _repository.GetRefreshTokenByTokenAsync(request.RefreshToken);
		
		if(queryResult is null)
		{
			return new() { Success = false };
		}
		RefreshToken mappedResult = _mapper.Map<RefreshToken>(queryResult);
		return new() { Success = true, Data = mappedResult };
	}
}
