using MediatR;
using MVCAPIDemo.Application.Domain;

namespace MVCAPIDemo.Application.Queries.Users;

public class GetUserQuery: IRequest<User>
{
    public int Id { get; set; }
}
