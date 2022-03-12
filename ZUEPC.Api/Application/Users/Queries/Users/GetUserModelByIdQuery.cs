using MediatR;
using ZUEPC.EvidencePublication.Base.Queries;

namespace ZUEPC.Api.Application.Users.Queries.Users;

public class GetUserModelByIdQuery:
	EPCSimpleQueryBase,
	IRequest<GetUserModelByIdQueryResponse>
{
}
