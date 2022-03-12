using AutoMapper;
using MediatR;
using Users.Base.Domain;
using ZUEPC.Api.Application.Users.Commands.UserRoles;
using ZUEPC.Api.Application.Users.Commands.Users;
using ZUEPC.DataAccess.Enums;
using ZUEPC.DataAccess.Models.Users;

namespace ZUEPC.Application.Auth.Commands.Users;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public RegisterUserCommandHandler(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}

	public async Task<RegisterUserCommandResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
	{
		if (request.Password is null)
		{
			return new() { Success = false };
		}

		CreateUserCommand createUserRequest = _mapper.Map<CreateUserCommand>(request);
		CreateUserCommandResponse response = await _mediator.Send(createUserRequest);
		if(!response.Success)
		{
			return new() { Success = false };
		}
		User newUser = response.Data;
		List<RoleType>? defaultUserRoles = new() { RoleType.USER };

		foreach (RoleType role in defaultUserRoles)
		{
			CreateUserRoleCommand createUserRoleCommand = new() { UserId = newUser.Id, RoleId = (long)role };
			await _mediator.Send(createUserRoleCommand);
		}

		return new() { Success = true, Data = response.Data };
	}
}
