using MediatR;
using ZUEPC.EvidencePublication.Base.Queries;

namespace ZUEPC.Application.Publications.Queries.Publictions;

public class GetPublicationQuery : 
	EPCSimpleQueryBase<long>,
	IRequest<GetPublicationQueryResponse>
{
}
