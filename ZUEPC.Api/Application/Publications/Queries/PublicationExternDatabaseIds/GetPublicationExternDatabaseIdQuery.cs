using MediatR;
using ZUEPC.EvidencePublication.Base.Queries;

namespace ZUEPC.Application.Publications.Queries.PublicationExternDatabaseIds;

public class GetPublicationExternDatabaseIdQuery : 
	EPCSimpleQueryBase,
	IRequest<GetPublicationExternDatabaseIdQueryResponse>
{
}
