using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Users;
using ZUEPC.DataAccess.Models.Users;
using ZUEPC.Users.Base.Domain;

namespace ZUEPC.Api.Application.Users.Commands.UserRoles;

public class CreateUserRoleCommandHandler : IRequestHandler<CreateUserRoleCommand, CreateUserRoleCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IUserRoleData _repository;

	public CreateUserRoleCommandHandler(IMapper mapper, IUserRoleData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<CreateUserRoleCommandResponse> Handle(CreateUserRoleCommand request, CancellationToken cancellationToken)
	{
		UserRoleModel insertModel = _mapper.Map<UserRoleModel>(request);
		insertModel.CreatedAt = DateTime.Now;
		long insertedId = await _repository.InsertModelAsync(insertModel);
		UserRole domain = _mapper.Map<UserRole>(insertModel);
		return new() { Success = true, Data = domain };
	}
}
