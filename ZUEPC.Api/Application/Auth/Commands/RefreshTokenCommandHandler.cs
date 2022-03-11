using AutoMapper;
using MediatR;
using ZUEPC.Auth.Services;

namespace ZUEPC.Application.Auth.Commands;

public class RefreshTokenCommandHandler: IRequestHandler<RefreshTokenCommand, RefreshTokenCommandResponse>
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
		var result = await _jwtAuthenticationService.VerifyAndGenerateTokenAsync(request.Token, request.RefreshToken);
		var response = _mapper.Map<RefreshTokenCommandResponse>(result);
		return response;
	}
}
