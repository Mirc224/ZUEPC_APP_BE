using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Details;

public class GetInstitutionDetailsQuery :
	EPCSimpleQueryBase<long>,
	IRequest<GetInstitutionDetailsQueryResponse>
{
}
