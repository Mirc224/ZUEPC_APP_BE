using MediatR;

namespace ZUEPC.Application.Users.Queries.Users;

public class GetUserQuery: IRequest<GetUserQueryResponse>
{
    public long Id { get; set; }
}
