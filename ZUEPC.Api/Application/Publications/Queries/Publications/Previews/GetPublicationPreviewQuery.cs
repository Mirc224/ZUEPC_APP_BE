using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.Publications.Queries.Publications.Previews;

public class GetPublicationPreviewQuery : 
	QueryWithIdBase<long>,
	IRequest<GetPublicationPreviewQueryResponse>
{
}
