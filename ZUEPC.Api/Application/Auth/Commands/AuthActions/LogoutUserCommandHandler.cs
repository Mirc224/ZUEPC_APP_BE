using AutoMapper;
using MediatR;
using System.Security.Cryptography;
using ZUEPC.Auth.Domain;
using ZUEPC.Auth.Services;

namespace ZUEPC.Application.Auth.Commands.AuthActions;

public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand, LogoutUserCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly AuthenticationService _jwtAuthenticationService;


	public LogoutUserCommandHandler(
		IMapper mapper,
		AuthenticationService jwtAuthenticationService)
	{
		_mapper = mapper;
		_jwtAuthenticationService = jwtAuthenticationService;
	}

	public async Task<LogoutUserCommandResponse> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
	{
		RevokeResult authResult = await _jwtAuthenticationService.RevokeUserTokenAsync(request.UserId, request?.JwtId);
		LogoutUserCommandResponse? response = _mapper.Map<LogoutUserCommandResponse>(authResult);
		return response;
	}
}
