using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.Persons.Queries.PersonNames;

public class GetPersonNameQuery:
	QueryWithIdBase<long>,
	IRequest<GetPersonNameQueryResponse>
{
}
