using MediatR;
using ZUEPC.EvidencePublication.Base.Queries;

namespace ZUEPC.Application.PublicationAuthors.Queries;

public class GetPublicationAuthorQuery : 
	EPCSimpleQueryBase,
	IRequest<GetPublicationAuthorQueryResponse>
{
}
