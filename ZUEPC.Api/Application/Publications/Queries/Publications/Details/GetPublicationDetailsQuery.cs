using MediatR;
using ZUEPC.EvidencePublication.Base.Queries;

namespace ZUEPC.Application.Publications.Queries.Publications.Details;

public class GetPublicationDetailsQuery :
	EPCSimpleQueryBase<long>,
	IRequest<GetPublicationDetailsQueryResponse>
{
}
