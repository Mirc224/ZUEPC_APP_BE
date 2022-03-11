using MediatR;
using ZUEPC.EvidencePublication.Base.Queries;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Details;

public class GetInstitutionDetailsQuery :
	EPCSimpleQueryBase,
	IRequest<GetInstitutionDetailsQueryResponse>
{
}
