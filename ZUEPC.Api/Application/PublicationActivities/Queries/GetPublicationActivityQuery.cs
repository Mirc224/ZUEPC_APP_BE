using MediatR;
using ZUEPC.EvidencePublication.Base.Queries;

namespace ZUEPC.Application.PublicationActivities.Queries;

public class GetPublicationActivityQuery : 
	EPCSimpleQueryBase<long>,
	IRequest<GetPublicationActivityQueryResponse>
{
}
