using MediatR;
using ZUEPC.EvidencePublication.Base.Queries;

namespace ZUEPC.Application.RelatedPublications.Queries;

public class GetRelatedPublicationQuery : 
	EPCSimpleQueryBase,
	IRequest<GetRelatedPublicationQueryResponse>
{
}
