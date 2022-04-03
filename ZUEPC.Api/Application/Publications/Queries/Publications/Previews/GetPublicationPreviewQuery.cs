using MediatR;
using ZUEPC.EvidencePublication.Base.Queries;

namespace ZUEPC.Application.Publications.Queries.Publications.Previews;

public class GetPublicationPreviewQuery : 
	EPCSimpleQueryBase<long>,
	IRequest<GetPublicationPreviewQueryResponse>
{
}
