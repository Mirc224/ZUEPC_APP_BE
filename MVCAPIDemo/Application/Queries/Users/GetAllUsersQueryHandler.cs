using AutoMapper;
using Dapper;
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
		var sqlBuilder = new SqlBuilder();
		sqlBuilder.Select("*");
		var usersModels = await _repository.GetUsersAsync(new {}, sqlBuilder);

		var users = _mapper.Map<List<User>>(usersModels);
		foreach(var user in users)
		{
			sqlBuilder = new SqlBuilder();
			sqlBuilder.Where("UserId = @UserId");
			var roles = await _repository.GetUserRolesAsync(new { UserId = user.Id}, sqlBuilder);
			user.Roles = roles.Select(x => x.Id).ToList();
		}
		var response = new GetAllUsersQueryResponse() { Success = true, Users = users};
        return response;
    }
}
