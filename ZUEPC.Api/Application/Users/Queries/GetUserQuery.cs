using MediatR;

namespace ZUEPC.Application.Users.Queries;

public class GetUserQuery: IRequest<GetUserQueryResponse>
{
    public int Id { get; set; }
}
