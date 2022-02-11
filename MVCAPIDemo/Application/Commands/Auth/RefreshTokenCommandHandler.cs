using AutoMapper;
using DataAccess.Data.User;
using MediatR;
using Microsoft.Extensions.Localization;
using MVCAPIDemo.Application.Services;
using MVCAPIDemo.Localization;

namespace MVCAPIDemo.Application.Commands.Auth;

public class RefreshTokenCommandHandler: IRequestHandler<RefreshTokenCommand, RefreshTokenCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly JwtAuthenticationService _jwtAuthenticationService;

	public RefreshTokenCommandHandler(IMapper mapper, JwtAuthenticationService jwtAuthenticationService)
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
