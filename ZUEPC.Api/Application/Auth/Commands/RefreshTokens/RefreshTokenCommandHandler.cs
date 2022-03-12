using AutoMapper;
using MediatR;
using ZUEPC.Auth.Domain;
using ZUEPC.Auth.Services;

namespace ZUEPC.Application.Auth.Commands.RefreshTokens;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly AuthenticationService _jwtAuthenticationService;

	public RefreshTokenCommandHandler(IMapper mapper, AuthenticationService jwtAuthenticationService)
	{
		_mapper = mapper;
		_jwtAuthenticationService = jwtAuthenticationService;
	}

	public async Task<RefreshTokenCommandResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
	{
		AuthResult? result = await _jwtAuthenticationService.VerifyAndGenerateTokenAsync(request.Token, request.RefreshToken);
		RefreshTokenCommandResponse? response = _mapper.Map<RefreshTokenCommandResponse>(result);
		return response;
	}
}
