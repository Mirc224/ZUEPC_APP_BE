using AutoMapper;
using MediatR;
using Users.Base.Application.Domain;
using ZUEPC.DataAccess.Data.Users;
using ZUEPC.DataAccess.Models.Users;

namespace ZUEPC.Application.Users.Queries;

public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, GetAllRolesQueryResponse>
{
	private readonly IRoleData _repository;
	private readonly IMapper _mapper;
	public GetAllRolesQueryHandler(IMapper mapper, IRoleData repository)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<GetAllRolesQueryResponse> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<RoleModel> roles = await _repository.GetAllAsync();
		IEnumerable<Role> mappedResponse = _mapper.Map<List<Role>>(roles);
		return new() { Success = true, Roles = mappedResponse};
	}
}
