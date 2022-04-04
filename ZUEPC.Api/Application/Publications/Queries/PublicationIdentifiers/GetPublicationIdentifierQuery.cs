using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.Publications.Queries.PublicationIdentifiers;

public class GetPublicationIdentifierQuery : 
	EPCQueryWithIdBase<long>,
	IRequest<GetPublicationIdentifierQueryResponse>
{
}
