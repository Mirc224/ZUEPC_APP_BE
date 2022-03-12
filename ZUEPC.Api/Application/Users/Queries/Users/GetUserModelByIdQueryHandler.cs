using DataAccess.Data.User;
using MediatR;
using ZUEPC.DataAccess.Models.Users;

namespace ZUEPC.Api.Application.Users.Queries.Users;

public class GetUserModelByIdQueryHandler
	: IRequestHandler<GetUserModelByIdQuery, GetUserModelByIdQueryResponse>
{
	private readonly IUserData _repository;

	public GetUserModelByIdQueryHandler(IUserData repository)
	{
		_repository = repository;
	}
	public async Task<GetUserModelByIdQueryResponse> Handle(GetUserModelByIdQuery request, CancellationToken cancellationToken)
	{
		UserModel? queryResponse = await _repository.GetModelByIdAsync(request.Id);
		if(queryResponse is null)
		{
			return new() { Success = false };
		}
		return new() { Success = true, Data = queryResponse };
	}
}
