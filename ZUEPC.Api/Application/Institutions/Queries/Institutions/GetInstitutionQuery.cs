using MediatR;
using ZUEPC.EvidencePublication.Base.Queries;

namespace ZUEPC.Application.Institutions.Queries.Institutions;

public class GetInstitutionQuery : 
	EPCSimpleQueryBase<long>,
	IRequest<GetInstitutionQueryResponse>
{
}
