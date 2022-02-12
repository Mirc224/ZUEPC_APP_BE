using MediatR;

namespace MVCAPIDemo.Users.Queries;

public class GetUserQuery: IRequest<GetUserQueryResponse>
{
    public int Id { get; set; }
}
