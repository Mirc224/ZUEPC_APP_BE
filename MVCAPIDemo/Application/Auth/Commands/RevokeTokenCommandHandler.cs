using AutoMapper;
using MediatR;
using MVCAPIDemo.Auth.Services;

namespace MVCAPIDemo.Auth.Commands;
public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, RevokeTokenCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly AuthenticationService _jwtAuthenticationService;

	public RevokeTokenCommandHandler(IMapper mapper, AuthenticationService jwtAuthenticationService)
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
