using MediatR;
using ZUEPC.EvidencePublication.Base.Queries;

namespace ZUEPC.Application.Persons.Queries.PersonNames;

public class GetPersonNameQuery:
	EPCSimpleQueryBase<long>,
	IRequest<GetPersonNameQueryResponse>
{
}
