using AutoMapper;
using DataAccess.Data.User;
using MediatR;
using MVCAPIDemo.Application.Domain;

namespace MVCAPIDemo.Application.Queries.Users;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, GetAllUsersQueryResponse>
{
    private readonly IUserData _repository;
    private readonly IMapper _mapper;
    public GetAllUsersQueryHandler(IMapper mapper,IUserData repository)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GetAllUsersQueryResponse> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var usersModels = await _repository.GetUsers();
		var users = _mapper.Map<List<User>>(usersModels);
		foreach(var user in users)
		{
			var roles = await _repository.GetUserRoles(user.Id);
			user.Roles = roles.Select(x => x.Id).ToList();
		}
		var response = new GetAllUsersQueryResponse() { Success = true, Users = users};
        return response;
    }
}
