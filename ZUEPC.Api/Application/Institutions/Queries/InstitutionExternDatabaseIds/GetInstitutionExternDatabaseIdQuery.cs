using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.Institutions.Queries.InstitutionExternDatabaseIds;

public class GetInstitutionExternDatabaseIdQuery : 
	EPCSimpleQueryBase<long>,
	IRequest<GetInstitutionExternDatabaseIdQueryResponse>
{
}
