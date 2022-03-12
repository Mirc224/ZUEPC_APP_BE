using AutoMapper;
using MediatR;
using ZUEPC.Auth.Domain;
using ZUEPC.Auth.Services;

namespace ZUEPC.Application.Auth.Commands.RefreshTokens;
public class RevokeRefreshTokenCommandHandler : IRequestHandler<RevokeRefreshTokenCommand, RevokeRefreshTokenCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly AuthenticationService _jwtAuthenticationService;

	public RevokeRefreshTokenCommandHandler(IMapper mapper, AuthenticationService jwtAuthenticationService)
	{
		_mapper = mapper;
		_jwtAuthenticationService = jwtAuthenticationService;
	}

	public async Task<RevokeRefreshTokenCommandResponse> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
	{
		RevokeResult result = await _jwtAuthenticationService.RevokeTokenAsync(request.RefreshToken);
		RevokeRefreshTokenCommandResponse? response = _mapper.Map<RevokeRefreshTokenCommandResponse>(result);
		return response;
	}
}
