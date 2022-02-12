using AutoMapper;
using MediatR;
using MVCAPIDemo.Auth.Services;
using System.Security.Cryptography;

namespace MVCAPIDemo.Auth.Commands;

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
        var authResult = await _jwtAuthenticationService.RevokeUserTokenAsync(request.UserId, request.JwtId);
		var response = _mapper.Map<LogoutUserCommandResponse>(authResult);
        return response;
    }

    private byte[] GetHash(string password, byte[] salt)
    {
        using HMACSHA512 hmac = new(salt);
        return hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); ;
    }

    private bool VerifyPasswordHash(string password, byte[] userPasswordHash, byte[] userPasswordSalt)
    {
        byte[] passwordHash = GetHash(password, userPasswordSalt);
        return Convert.ToBase64String(passwordHash) == Convert.ToBase64String(userPasswordHash);
    }
}
