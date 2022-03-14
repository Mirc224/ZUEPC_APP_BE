using AutoMapper;
using DataAccess.Data.User;
using MediatR;
using Users.Base.Domain;
using ZUEPC.Api.Common.CQRS.QueryHandlers;
using ZUEPC.DataAccess.Filters;
using ZUEPC.DataAccess.Models.Users;

namespace ZUEPC.Api.Application.Users.Queries.Users;

public class GetAllUsersQueryHandler :
	GetModelsWithFiltersPagedQueryHandlerBase<IUserData, User, UserModel, UserFilter>,
	IRequestHandler<GetAllUsersQuery, GetAllUsersQueryResponse>
{
	public GetAllUsersQueryHandler(IMapper mapper, IUserData repository)
		:base(mapper, repository) { }
	
	public async Task<GetAllUsersQueryResponse> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
	{
		if(request.QueryFilter is null)
		{
			return await ProcessQueryAsync<GetAllUsersQuery, GetAllUsersQueryResponse>(request);
		}
		return await ProcessQueryWithFilterAsync<GetAllUsersQuery, GetAllUsersQueryResponse>(request);
	}
}
