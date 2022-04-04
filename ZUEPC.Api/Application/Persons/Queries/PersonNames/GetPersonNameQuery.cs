using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.Persons.Queries.PersonNames;

public class GetPersonNameQuery:
	EPCQueryWithIdBase<long>,
	IRequest<GetPersonNameQueryResponse>
{
}
