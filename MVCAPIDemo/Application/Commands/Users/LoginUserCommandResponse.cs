using MVCAPIDemo.Application.Responses;

namespace MVCAPIDemo.Application.Commands.Users;

public class LoginUserCommandResponse : CQRSBaseResponse
{
    public string? Token { get; set; }
}
