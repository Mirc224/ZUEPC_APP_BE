using MediatR;
using ZUEPC.EvidencePublication.Base.Queries;

namespace ZUEPC.Application.Institutions.Queries.InstitutionExternDatabaseIds;

public class GetInstitutionExternDatabaseIdQuery : 
	EPCSimpleQueryBase,
	IRequest<GetInstitutionExternDatabaseIdQueryResponse>
{
}
