using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.Institutions.Queries.InstitutionExternDatabaseIds;

public class GetInstitutionExternDatabaseIdQuery : 
	QueryWithIdBase<long>,
	IRequest<GetInstitutionExternDatabaseIdQueryResponse>
{
}
