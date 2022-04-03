using MediatR;
using ZUEPC.EvidencePublication.Base.Queries;

namespace ZUEPC.Application.Persons.Queries.Persons.Details;

public class GetPersonDetailsQuery :
	EPCSimpleQueryBase<long>,
	IRequest<GetPersonDetailsQueryResponse>
{
}
