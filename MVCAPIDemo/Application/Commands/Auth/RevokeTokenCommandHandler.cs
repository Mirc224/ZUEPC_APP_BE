using AutoMapper;
using MediatR;
using MVCAPIDemo.Application.Services;

namespace MVCAPIDemo.Application.Commands.Auth;

public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, RevokeTokenCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly JwtAuthenticationService _jwtAuthenticationService;

	public RevokeTokenCommandHandler(IMapper mapper, JwtAuthenticationService jwtAuthenticationService)
	{
		_mapper = mapper;
		_jwtAuthenticationService = jwtAuthenticationService;
	}

	public async Task<RevokeTokenCommandResponse> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
	{
		var result = await _jwtAuthenticationService.RevokeTokenAsync(request.RefreshToken);
		var response = _mapper.Map<RevokeTokenCommandResponse>(result);
		return response;
	}
}
