using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.PublicationAuthors.Queries;

public class GetPublicationAuthorQuery : 
	EPCQueryWithIdBase<long>,
	IRequest<GetPublicationAuthorQueryResponse>
{
}
