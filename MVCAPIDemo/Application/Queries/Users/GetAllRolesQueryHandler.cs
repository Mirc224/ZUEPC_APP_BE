using AutoMapper;
using DataAccess.Data.User;
using MediatR;
using MVCAPIDemo.Application.Domain;

namespace MVCAPIDemo.Application.Queries.Users;

public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, GetAllRolesQueryResponse>
{
    private readonly IUserData _repository;
    private readonly IMapper _mapper;
    public GetAllRolesQueryHandler(IMapper mapper, IUserData repository)
    {
        _repository = repository;
        _mapper = mapper;
    }

	public async Task<GetAllRolesQueryResponse> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
	{
		var roles = await _repository.GetRoles();
		var response = new GetAllRolesQueryResponse() { Success = true, Roles = _mapper.Map<List<Role>>(roles) };
        return response;
    }
}
