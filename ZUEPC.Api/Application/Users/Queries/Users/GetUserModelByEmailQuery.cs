using MediatR;

namespace ZUEPC.Api.Application.Users.Queries.Users;

public class GetUserModelByEmailQuery : IRequest<GetUserModelByEmailQueryResponse>
{
	public string? UserEmail { get; set; }
}
