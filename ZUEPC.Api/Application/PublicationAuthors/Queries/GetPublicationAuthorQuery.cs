using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.PublicationAuthors.Queries;

public class GetPublicationAuthorQuery : 
	EPCSimpleQueryBase<long>,
	IRequest<GetPublicationAuthorQueryResponse>
{
}
