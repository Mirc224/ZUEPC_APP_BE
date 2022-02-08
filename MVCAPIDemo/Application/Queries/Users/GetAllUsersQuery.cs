using MediatR;
using MVCAPIDemo.Application.Domain;

namespace MVCAPIDemo.Application.Queries.Users;

public class GetAllUsersQuery: IRequest<IEnumerable<User>>
{
}
