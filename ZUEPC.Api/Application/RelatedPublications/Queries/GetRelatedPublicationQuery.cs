using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.RelatedPublications.Queries;

public class GetRelatedPublicationQuery : 
	EPCQueryWithIdBase<long>,
	IRequest<GetRelatedPublicationQueryResponse>
{
}
