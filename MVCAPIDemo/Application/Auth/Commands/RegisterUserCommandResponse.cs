using ZUEPC.Responses;
using Users.Base.Domain;

namespace ZUEPC.Application.Auth.Commands;

public class RegisterUserCommandResponse : ResponseBase
{
	public User? CreatedUser { get; set; }
}
