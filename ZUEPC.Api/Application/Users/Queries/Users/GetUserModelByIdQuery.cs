using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Api.Application.Users.Queries.Users;

public class GetUserModelByIdQuery:
	EPCSimpleQueryBase<long>,
	IRequest<GetUserModelByIdQueryResponse>
{
}
