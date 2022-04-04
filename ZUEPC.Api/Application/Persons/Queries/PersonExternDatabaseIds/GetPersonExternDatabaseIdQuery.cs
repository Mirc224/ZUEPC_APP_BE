using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.Persons.Queries.PersonExternDatabaseIds;

public class GetPersonExternDatabaseIdQuery :
	EPCSimpleQueryBase<long>,
	IRequest<GetPersonExternDatabaseIdQueryResponse>
{
}
