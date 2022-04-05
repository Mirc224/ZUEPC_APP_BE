using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.Persons.Queries.Persons.Details;

public class GetPersonDetailsQuery :
	QueryWithIdBase<long>,
	IRequest<GetPersonDetailsQueryResponse>
{
}
