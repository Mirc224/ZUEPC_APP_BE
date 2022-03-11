using MediatR;
using ZUEPC.EvidencePublication.Base.Queries;

namespace ZUEPC.Application.Publications.Queries.PublicationNames;

public class GetPublicationNameQuery : 
	EPCSimpleQueryBase,
	IRequest<GetPublicationNameQueryResponse>
{
}
