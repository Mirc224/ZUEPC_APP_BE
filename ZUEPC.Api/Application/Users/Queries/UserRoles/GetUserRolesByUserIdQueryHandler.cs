using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Users;
using ZUEPC.DataAccess.Models.Users;
using ZUEPC.Users.Domain;

namespace ZUEPC.Api.Application.Users.Queries.UserRoles;

public class GetUserRolesByUserIdQueryHandler : IRequestHandler<GetUserRolesByUserIdQuery, GetUserRolesByUserIdQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IUserRoleData _repository;

	public GetUserRolesByUserIdQueryHandler(IMapper mapper, IUserRoleData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<GetUserRolesByUserIdQueryResponse> Handle(GetUserRolesByUserIdQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<UserRoleModel> queryResult = await _repository.GetUserRolesByUserIdAsync(request.UserId);
		List<UserRole> mappedResult = _mapper.Map<List<UserRole>>(queryResult);
		return new() { Success = true, Data = mappedResult };
	}
}
