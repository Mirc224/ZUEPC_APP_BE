using MediatR;
using ZUEPC.EvidencePublication.Base.Queries;

namespace ZUEPC.Application.Publications.Queries.PublicationIdentifiers;

public class GetPublicationIdentifierQuery : 
	EPCSimpleQueryBase<long>,
	IRequest<GetPublicationIdentifierQueryResponse>
{
}
