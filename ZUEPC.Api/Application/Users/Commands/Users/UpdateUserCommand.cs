using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Api.Application.Users.Commands.Users;

public class UpdateUserCommand : 
	UpdateCommandWithIdBase<long>,
	IRequest<UpdateUserCommandResponse>
{
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
}
