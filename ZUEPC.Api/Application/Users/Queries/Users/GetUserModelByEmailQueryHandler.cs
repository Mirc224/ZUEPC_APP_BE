using DataAccess.Data.User;
using MediatR;
using ZUEPC.DataAccess.Models.Users;

namespace ZUEPC.Api.Application.Users.Queries.Users;

public class GetUserModelByEmailQueryHandler
	: IRequestHandler<GetUserModelByEmailQuery, GetUserModelByEmailQueryResponse>
{
	private readonly IUserData _repository;

	public GetUserModelByEmailQueryHandler(IUserData repository)
	{
		_repository = repository;
	}

	public async Task<GetUserModelByEmailQueryResponse> Handle(GetUserModelByEmailQuery request, CancellationToken cancellationToken)
	{
		if (request.UserEmail is null)
		{
			return new() { Success = false };
		}
		UserModel? queryResult = await _repository.GetUserByEmailAsync(request.UserEmail);

		if(queryResult is null)
		{
			return new() { Success=false };
		}
		return new() { Success = true, Data = queryResult };
	}
}
