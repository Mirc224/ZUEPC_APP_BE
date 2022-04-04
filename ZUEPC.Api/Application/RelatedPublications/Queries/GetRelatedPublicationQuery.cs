using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.RelatedPublications.Queries;

public class GetRelatedPublicationQuery : 
	EPCSimpleQueryBase<long>,
	IRequest<GetRelatedPublicationQueryResponse>
{
}
