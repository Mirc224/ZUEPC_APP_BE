using MediatR;

namespace ZUEPC.Application.Users.Queries;

public class GetUserQuery: IRequest<GetUserQueryResponse>
{
    public long Id { get; set; }
}
