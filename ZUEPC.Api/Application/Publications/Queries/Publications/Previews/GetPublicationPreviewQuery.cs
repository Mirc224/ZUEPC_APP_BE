using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.Publications.Queries.Publications.Previews;

public class GetPublicationPreviewQuery : 
	EPCQueryWithIdBase<long>,
	IRequest<GetPublicationPreviewQueryResponse>
{
}
