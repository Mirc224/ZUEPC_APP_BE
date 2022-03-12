using AutoMapper;
using DataAccess.Data.User;
using MediatR;
using Users.Base.Domain;
using ZUEPC.Common.CQRS.QueryHandlers;
using ZUEPC.DataAccess.Models.Users;

namespace ZUEPC.Api.Application.Users.Queries.Users;

public class GetAllUsersQueryHandler :
	GetAllPagedSimpleModelQueryHandlerBase<User, UserModel>,
	IRequestHandler<GetAllUsersQuery, GetAllUsersQueryResponse>
{
	public GetAllUsersQueryHandler(IMapper mapper, IUserData repository)
		:base(mapper, repository) { }
	
	public async Task<GetAllUsersQueryResponse> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
	{
		return await ProcessQueryAsync<GetAllUsersQuery, GetAllUsersQueryResponse>(request);
	}
}
