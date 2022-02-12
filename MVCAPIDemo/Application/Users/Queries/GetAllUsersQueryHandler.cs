﻿using AutoMapper;
using Dapper;
using DataAccess.Data.User;
using MediatR;
using Users.Base.Domain;

namespace MVCAPIDemo.Users.Queries;

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
		var usersModels = await _repository.GetUsersAsync();

		var users = _mapper.Map<List<User>>(usersModels);
		foreach(var user in users)
		{
			var roles = await _repository.GetUserRolesAsync(user.Id);
			user.Roles = roles.Select(x => x.Id).ToList();
		}
		var response = new GetAllUsersQueryResponse() { Success = true, Users = users};
        return response;
    }
}