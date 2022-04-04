using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.PublicationActivities.Queries;

public class GetPublicationActivityQuery : 
	EPCSimpleQueryBase<long>,
	IRequest<GetPublicationActivityQueryResponse>
{
}
