using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.Publications.Queries.Publications.Details;

public class GetPublicationDetailsQuery :
	QueryWithIdBase<long>,
	IRequest<GetPublicationDetailsQueryResponse>
{
}
