using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.PublicationAuthors.Queries;

public class GetPublicationAuthorQuery : 
	QueryWithIdBase<long>,
	IRequest<GetPublicationAuthorQueryResponse>
{
}
