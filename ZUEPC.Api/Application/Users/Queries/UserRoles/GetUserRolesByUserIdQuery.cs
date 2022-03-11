using MediatR;
using ZUEPC.EvidencePublication.Base.Queries;

namespace ZUEPC.Api.Application.Users.Queries.UserRoles;

public class GetUserRolesByUserIdQuery :
	IRequest<GetUserRolesByUserIdQueryResponse>
{
	public long UserId { get; set; }
}
