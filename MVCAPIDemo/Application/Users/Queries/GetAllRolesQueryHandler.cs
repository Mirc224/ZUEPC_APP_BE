using AutoMapper;
using DataAccess.Data.User;
using MediatR;
using Users.Base.Application.Domain;

namespace MVCAPIDemo.Users.Queries;

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
		var roles = await _repository.GetRolesAsync();
		var response = new GetAllRolesQueryResponse() { Success = true, Roles = _mapper.Map<List<Role>>(roles) };
        return response;
    }
}
