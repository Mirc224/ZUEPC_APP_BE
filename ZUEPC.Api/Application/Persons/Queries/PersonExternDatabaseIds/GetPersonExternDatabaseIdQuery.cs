using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.Persons.Queries.PersonExternDatabaseIds;

public class GetPersonExternDatabaseIdQuery :
	EPCQueryWithIdBase<long>,
	IRequest<GetPersonExternDatabaseIdQueryResponse>
{
}
