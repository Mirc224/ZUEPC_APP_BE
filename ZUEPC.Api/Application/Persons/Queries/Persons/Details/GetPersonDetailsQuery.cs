using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.Persons.Queries.Persons.Details;

public class GetPersonDetailsQuery :
	EPCQueryWithIdBase<long>,
	IRequest<GetPersonDetailsQueryResponse>
{
}
